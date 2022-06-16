using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Applications.Order.CommonDto;
using e_shop_api.DataBase;
using e_shop_api.Enumeration;
using e_shop_api.Extensions;
using e_shop_api.Utility.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Order.Query
{
    public class QueryOrdersHandler : IRequestHandler<QueryOrdersRequest, QueryOrdersResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<QueryOrdersHandler> _logger;
        private readonly IPageUtility _pageUtility;

        public QueryOrdersHandler(EShopDbContext eShopDbContext, ILogger<QueryOrdersHandler> logger,
            IPageUtility pageUtility)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
            _pageUtility = pageUtility;
        }

        public async Task<QueryOrdersResponse> Handle(QueryOrdersRequest request, CancellationToken cancellationToken)
        {
            var totalCount = await GetTotalCount(request, cancellationToken);
            if (totalCount == 0)
            {
                return new QueryOrdersResponse()
                {
                    Success = true,
                    Message = "沒有資料",
                    OrderInfos = new List<OrderInfo>(),
                    Pagination = new Pagination()
                };
            }

            // 分頁固定筆數
            const int pageSize = 10;

            var ordersQuery = _eShopDbContext.Orders.AsQueryable();

            // 過濾查詢
            ordersQuery = OrdersFilter(request, ordersQuery);

            var orders = await ordersQuery
                .OrderByDescending(s => s.CreationTime)
                .Page(request.Page, pageSize)
                .ToListAsync(cancellationToken: cancellationToken);

            // order
            var orderInfos = orders
                .Select(s => new OrderInfo()
                {
                    OrderId = s.Id,
                    UserId = s.UserId,
                    IsPaid = s.IsPaid,
                    PaymentMethod = s.PaymentMethod,
                    CreateDateTime = s.CreationTime.ToTimeStamp(),
                    PaidDateTime = s.PaidDateTime?.ToTimeStamp() ?? 0,
                    TotalAmount = s.TotalAmount,
                    UserName = s.UserName,
                    Address = s.Address,
                    Email = s.Email,
                    Tel = s.Tel,
                    Message = s.Message
                }).ToList();

            // products
            var products = await _eShopDbContext.Products
                .ToListAsync(cancellationToken: cancellationToken);

            // order detail
            foreach (var order in orderInfos)
            {
                var orderDetail = (await _eShopDbContext.OrderDetails
                        .Where(s => s.OrderId == order.OrderId)
                        .ToListAsync(cancellationToken: cancellationToken))
                    .Select(s => new OrderDetailInfo()
                    {
                        ProductTitle = products.Single(n => n.Id == s.ProductId).Title,
                        ProductUnit = products.Single(n => n.Id == s.ProductId).Unit,
                        Qty = s.Qty
                    }).ToList();

                order.OrderDetailInfos = orderDetail;
            }

            return new QueryOrdersResponse()
            {
                Success = true,
                Message = "查詢成功",
                OrderInfos = orderInfos.ToList(),
                Pagination = _pageUtility.GetPagination(totalCount, request.Page, pageSize)
            };
        }

        private async Task<int> GetTotalCount(QueryOrdersRequest request, CancellationToken cancellationToken)
        {
            var orders = _eShopDbContext.Orders.AsQueryable();
            orders = OrdersFilter(request, orders);
            return await orders.CountAsync(cancellationToken);
        }

        private static IQueryable<DataBase.Models.Order> OrdersFilter(QueryOrdersRequest request,
            IQueryable<DataBase.Models.Order> orders)
        {
            if (string.IsNullOrWhiteSpace(request.StartDate) == false &&
                string.IsNullOrWhiteSpace(request.EndDate) == false)
            {
                var startDate = request.StartDate.ToDate();
                var endDate = request.EndDate.ToDate();
                orders = orders.Where(s =>
                    s.CreationTime >= startDate && s.CreationTime <= endDate);
            }

            var paymentMethodString = GetPaymentMethodQueryString(request.PaymentMethod);
            if (paymentMethodString != "")
            {
                orders = orders.Where(s => s.PaymentMethod == paymentMethodString);
            }

            return orders;
        }

        private static string GetPaymentMethodQueryString(int paymentMethod)
        {
            return paymentMethod switch
            {
                1 => "",
                2 => PaymentMethod.CashOnDelivery.ToString(),
                3 => PaymentMethod.CreditCardPayment.ToString(),
                _ => ""
            };
        }
    }
}
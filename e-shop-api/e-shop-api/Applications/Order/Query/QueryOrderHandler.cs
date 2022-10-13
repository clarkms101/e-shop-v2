using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Applications.Order.CommonDto;
using e_shop_api.DataBase;
using e_shop_api.Enumeration;
using e_shop_api.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Order.Query
{
    public class QueryOrderHandler : IRequestHandler<QueryOrderRequest, QueryOrderResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<QueryOrderHandler> _logger;

        public QueryOrderHandler(EShopDbContext eShopDbContext, ILogger<QueryOrderHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
        }

        public async Task<QueryOrderResponse> Handle(QueryOrderRequest request, CancellationToken cancellationToken)
        {
            var selectOrder = await _eShopDbContext.Orders.Where(s => s.SerialNumber == request.SerialNumber)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            if (selectOrder == null)
            {
                return new QueryOrderResponse()
                {
                    Success = false,
                    Message = "查無該筆資料!"
                };
            }

            // products
            var products = await _eShopDbContext.Products
                .ToListAsync(cancellationToken: cancellationToken);

            // order detail
            var orderDetail = (await _eShopDbContext.OrderDetails
                    .Where(s => s.OrderId == selectOrder.Id)
                    .ToListAsync(cancellationToken: cancellationToken))
                .Select(s => new OrderDetailInfo()
                {
                    ProductTitle = products.Single(n => n.Id == s.ProductId).Title,
                    ProductUnit = products.Single(n => n.Id == s.ProductId).Unit,
                    ProductPrice = products.Single(n => n.Id == s.ProductId).Price,
                    ImageUrl = products.Single(n => n.Id == s.ProductId).ImageUrl,
                    Qty = s.Qty
                }).ToList();

            var originTotalAmount = orderDetail.Sum(s => (s.ProductPrice * s.Qty));
            
            return new QueryOrderResponse()
            {
                Success = true,
                Message = "查詢成功!",
                OrderInfo = new OrderInfo()
                {
                    OrderId = selectOrder.Id,
                    UserId = selectOrder.UserId,
                    IsPaid = selectOrder.IsPaid,
                    OrderStatus = selectOrder.OrderStatus.FromStringToEnum<OrderStatus>().GetDescriptionText(),
                    PaymentMethod = selectOrder.PaymentMethod.FromStringToEnum<PaymentMethod>().GetDescriptionText(),
                    CreateDateTime = selectOrder.CreationTime.ToTimeStamp(),
                    PaidDateTime = selectOrder.PaidDateTime?.ToTimeStamp(),
                    OriginTotalAmount = originTotalAmount,
                    TotalAmount = selectOrder.TotalAmount,
                    UserName = selectOrder.UserName,
                    Address = selectOrder.Address,
                    Email = selectOrder.Email,
                    Tel = selectOrder.Tel,
                    Message = selectOrder.Message,
                    OrderDetailInfos = orderDetail
                }
            };
        }
    }
}
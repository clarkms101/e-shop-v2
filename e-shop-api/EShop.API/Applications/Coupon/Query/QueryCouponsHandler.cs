using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Extensions;
using e_shop_api.Extensions;
using e_shop_api.Utility.Interface;
using EShop.Entity.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Coupon.Query
{
    public class QueryCouponsHandler : IRequestHandler<QueryCouponsRequest, QueryCouponsResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<QueryCouponsHandler> _logger;
        private readonly IPageUtility _pageUtility;

        public QueryCouponsHandler(EShopDbContext eShopDbContext, ILogger<QueryCouponsHandler> logger,
            IPageUtility pageUtility)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
            _pageUtility = pageUtility;
        }

        public async Task<QueryCouponsResponse> Handle(QueryCouponsRequest request, CancellationToken cancellationToken)
        {
            var totalCount = await _eShopDbContext.Coupons.CountAsync(cancellationToken: cancellationToken);

            if (request.Page == 0 || totalCount == 0)
            {
                return new QueryCouponsResponse()
                {
                    Success = false,
                    Message = "沒有資料",
                    Coupons = new List<CommonDto.Coupon>(),
                    Pagination = new Pagination()
                };
            }

            var coupons = await _eShopDbContext.Coupons
                .OrderBy(s => s.Id)
                .Page(request.Page, request.PageSize)
                .ToListAsync(cancellationToken: cancellationToken);

            return new QueryCouponsResponse()
            {
                Success = true,
                Message = "查詢成功",
                Coupons = coupons
                    .Select(s => new CommonDto.Coupon()
                    {
                        CouponCode = s.CouponCode,
                        CouponId = s.Id,
                        DueDateTimeStamp = s.DueDateTime.ToTimeStamp(),
                        IsEnabled = s.IsEnabled,
                        Percent = s.Percent,
                        Title = s.Title
                    }).ToList(),
                Pagination = _pageUtility.GetPagination(totalCount, request.Page, request.PageSize)
            };
        }
    }
}
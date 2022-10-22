using System;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Core.Extensions;
using e_shop_api.Extensions;
using EShop.Entity.DataBase;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Coupon.Command.Create
{
    public class CreateCouponHandler : IRequestHandler<CreateCouponRequest, CreateCouponResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<CreateCouponHandler> _logger;

        public CreateCouponHandler(EShopDbContext eShopDbContext, ILogger<CreateCouponHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
        }

        public async Task<CreateCouponResponse> Handle(CreateCouponRequest request, CancellationToken cancellationToken)
        {
            var dueDate = request.Coupon.DueDateTimeStamp.ToDateTime();
            var newCoupon = new EShop.Entity.DataBase.Models.Coupon()
            {
                Id = request.Coupon.CouponId,
                Title = request.Coupon.Title,
                CouponCode = request.Coupon.CouponCode,
                Percent = request.Coupon.Percent,
                IsEnabled = request.Coupon.IsEnabled,
                // 固定該日的 23:59:59
                DueDateTime = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, 23, 59, 59),
                // system
                CreatorUserId = request.SystemUserId,
                CreationTime = DateTime.Now
            };

            await _eShopDbContext.Coupons.AddAsync(newCoupon, cancellationToken);
            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            return new CreateCouponResponse()
            {
                Success = true,
                Message = "新增成功!"
            };
        }
    }
}
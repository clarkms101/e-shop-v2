using System;
using System.Threading;
using System.Threading.Tasks;
using e_shop_api.DataBase;
using e_shop_api.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Coupon.Command.Update
{
    public class UpdateCouponHandler : IRequestHandler<UpdateCouponRequest, UpdateCouponResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<UpdateCouponHandler> _logger;

        public UpdateCouponHandler(EShopDbContext eShopDbContext, ILogger<UpdateCouponHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
        }

        public async Task<UpdateCouponResponse> Handle(UpdateCouponRequest request, CancellationToken cancellationToken)
        {
            var dueDate = request.Coupon.DueDateTimeStamp.ToDateTime();
            var oldCoupon = await _eShopDbContext.Coupons.FindAsync(request.Coupon.CouponId);
            if (oldCoupon == null)
            {
                return new UpdateCouponResponse()
                {
                    Success = false,
                    Message = "查無該筆資料!"
                };
            }

            oldCoupon.Title = request.Coupon.Title;
            oldCoupon.CouponCode = request.Coupon.CouponCode;
            oldCoupon.Percent = request.Coupon.Percent;
            oldCoupon.IsEnabled = request.Coupon.IsEnabled;
            // 固定該日的 23:59:59
            oldCoupon.DueDateTime = new DateTime(dueDate.Year, dueDate.Month, dueDate.Day, 23, 59, 59);

            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            return new UpdateCouponResponse()
            {
                Success = true,
                Message = "更新成功!"
            };
        }
    }
}
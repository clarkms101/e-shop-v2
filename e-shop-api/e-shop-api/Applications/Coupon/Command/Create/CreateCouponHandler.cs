using System.Threading;
using System.Threading.Tasks;
using e_shop_api.DataBase;
using e_shop_api.Extensions;
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
            var newCoupon = new DataBase.Models.Coupon()
            {
                Id = request.Coupon.CouponId,
                Title = request.Coupon.Title,
                CouponCode = request.Coupon.CouponCode,
                Percent = request.Coupon.Percent,
                IsEnabled = request.Coupon.IsEnabled,
                DueDateTime = request.Coupon.DueDateTimeStamp.ToDateTime()
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
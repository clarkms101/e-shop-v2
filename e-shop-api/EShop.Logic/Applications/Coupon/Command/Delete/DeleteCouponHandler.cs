using EShop.Entity.DataBase;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Coupon.Command.Delete
{
    public class DeleteCouponHandler : IRequestHandler<DeleteCouponRequest, DeleteCouponResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly ILogger<DeleteCouponHandler> _logger;

        public DeleteCouponHandler(EShopDbContext eShopDbContext, ILogger<DeleteCouponHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _logger = logger;
        }

        public async Task<DeleteCouponResponse> Handle(DeleteCouponRequest request, CancellationToken cancellationToken)
        {
            var selectCoupon = await _eShopDbContext.Coupons.FindAsync(request.CouponId);
            if (selectCoupon == null)
            {
                return new DeleteCouponResponse()
                {
                    Success = false,
                    Message = "查無該筆資料!"
                };
            }

            _eShopDbContext.Coupons.Remove(selectCoupon);
            await _eShopDbContext.SaveChangesAsync(cancellationToken);

            return new DeleteCouponResponse()
            {
                Success = true,
                Message = "刪除成功!"
            };
        }
    }
}
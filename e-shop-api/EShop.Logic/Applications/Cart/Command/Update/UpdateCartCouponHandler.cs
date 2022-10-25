using e_shop_api.Core.Const;
using EShop.Cache.Interface;
using EShop.Entity.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Cart.Command.Update
{
    public class UpdateCartCouponHandler : IRequestHandler<UpdateCartCouponRequest, UpdateCartCouponResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly IShoppingCartCacheUtility _shoppingCartCacheUtility;
        private readonly ILogger<UpdateCartCouponHandler> _logger;

        public UpdateCartCouponHandler(EShopDbContext eShopDbContext, IShoppingCartCacheUtility shoppingCartCacheUtility,
            ILogger<UpdateCartCouponHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _shoppingCartCacheUtility = shoppingCartCacheUtility;
            _logger = logger;
        }

        public async Task<UpdateCartCouponResponse> Handle(UpdateCartCouponRequest request,
            CancellationToken cancellationToken)
        {
            var coupon = await _eShopDbContext.Coupons
                .SingleOrDefaultAsync(s => s.CouponCode == request.Coupon.CouponCode,
                    cancellationToken: cancellationToken);

            if (coupon == null)
            {
                return new UpdateCartCouponResponse()
                {
                    Success = false,
                    Message = "找不到優惠券!"
                };
            }

            if (coupon.IsEnabled == false)
            {
                return new UpdateCartCouponResponse()
                {
                    Success = false,
                    Message = "優惠券尚未啟用!"
                };
            }

            if (coupon.DueDateTime <= DateTime.Now)
            {
                return new UpdateCartCouponResponse()
                {
                    Success = false,
                    Message = "優惠券已經過期!"
                };
            }

            var setIsSuccess = _shoppingCartCacheUtility.SetCouponIdToCart(CartInfo.DefaultCartId, coupon.Id);

            if (setIsSuccess)
            {
                return new UpdateCartCouponResponse()
                {
                    Success = true,
                    Message = $"已套用優惠券 {request.Coupon.CouponCode}"
                };
            }

            return new UpdateCartCouponResponse()
            {
                Success = false,
                Message = "套用優惠券失敗!"
            };
        }
    }
}
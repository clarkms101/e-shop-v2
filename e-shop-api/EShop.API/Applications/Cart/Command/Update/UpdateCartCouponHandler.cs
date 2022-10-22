using System.Threading;
using System.Threading.Tasks;
using e_shop_api.Utility.Const;
using e_shop_api.Utility.Interface;
using EShop.Entity.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace e_shop_api.Applications.Cart.Command.Update
{
    public class UpdateCartCouponHandler : IRequestHandler<UpdateCartCouponRequest, UpdateCartCouponResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly IShoppingCartUtility _shoppingCartUtility;
        private readonly ILogger<UpdateCartCouponHandler> _logger;

        public UpdateCartCouponHandler(EShopDbContext eShopDbContext, IShoppingCartUtility shoppingCartUtility,
            ILogger<UpdateCartCouponHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _shoppingCartUtility = shoppingCartUtility;
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

            // todo 過期優惠券檢查

            var setIsSuccess = _shoppingCartUtility.SetCouponIdToCart(CartInfo.DefaultCartId, coupon.Id);

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
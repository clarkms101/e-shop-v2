using e_shop_api.Applications.Cart.CommonDto;
using MediatR;

namespace e_shop_api.Applications.Cart.Command.Update
{
    public class UpdateCartCouponRequest : BaseCommandRequest, IRequest<UpdateCartCouponResponse>
    {
        public CartCoupon Coupon { get; set; }
    }
}
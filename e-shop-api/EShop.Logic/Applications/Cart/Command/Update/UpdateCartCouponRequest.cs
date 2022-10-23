using e_shop_api.Core.Dto;
using EShop.Logic.Applications.Cart.CommonDto;
using MediatR;

namespace EShop.Logic.Applications.Cart.Command.Update
{
    public class UpdateCartCouponRequest : BaseCommandRequest, IRequest<UpdateCartCouponResponse>
    {
        public CartCoupon Coupon { get; set; }
    }
}
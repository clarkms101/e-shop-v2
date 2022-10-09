using MediatR;

namespace e_shop_api.Applications.Coupon.Command.Update
{
    public class UpdateCouponRequest : BaseCommandRequest, IRequest<UpdateCouponResponse>
    {
        public CommonDto.Coupon Coupon { get; set; }
    }
}
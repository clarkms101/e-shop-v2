using MediatR;

namespace e_shop_api.Applications.Coupon.Command.Create
{
    public class CreateCouponRequest : IRequest<CreateCouponResponse>
    {
        public CommonDto.Coupon Coupon { get; set; }
    }
}
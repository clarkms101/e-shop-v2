using MediatR;

namespace e_shop_api.Applications.Coupon.Command.Delete
{
    public class DeleteCouponRequest : IRequest<DeleteCouponResponse>
    {
        public int CouponId { get; set; }
    }
}
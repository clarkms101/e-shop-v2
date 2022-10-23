using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Coupon.Command.Delete
{
    public class DeleteCouponRequest : BaseCommandRequest, IRequest<DeleteCouponResponse>
    {
        public int CouponId { get; set; }
    }
}
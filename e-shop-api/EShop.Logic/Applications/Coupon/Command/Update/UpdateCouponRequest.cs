using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Coupon.Command.Update
{
    public class UpdateCouponRequest : BaseCommandRequest, IRequest<UpdateCouponResponse>
    {
        public CommonDto.Coupon Coupon { get; set; }
    }
}
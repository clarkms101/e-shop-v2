using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Coupon.Command.Create
{
    public class CreateCouponRequest : BaseCommandRequest, IRequest<CreateCouponResponse>
    {
        public CommonDto.Coupon Coupon { get; set; }
    }
}
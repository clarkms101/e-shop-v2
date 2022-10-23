using e_shop_api.Applications.Cart.CommonDto;
using e_shop_api.Core.Dto;
using MediatR;

namespace e_shop_api.Applications.Cart.Command.Create
{
    public class CreateCartDetailRequest : BaseCommandRequest, IRequest<CreateCartDetailResponse>
    {
        public CartDetail CartDetail { get; set; }
    }
}
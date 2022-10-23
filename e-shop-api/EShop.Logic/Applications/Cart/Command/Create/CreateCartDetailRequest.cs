using e_shop_api.Core.Dto;
using EShop.Logic.Applications.Cart.CommonDto;
using MediatR;

namespace EShop.Logic.Applications.Cart.Command.Create
{
    public class CreateCartDetailRequest : BaseCommandRequest, IRequest<CreateCartDetailResponse>
    {
        public CartDetail CartDetail { get; set; }
    }
}
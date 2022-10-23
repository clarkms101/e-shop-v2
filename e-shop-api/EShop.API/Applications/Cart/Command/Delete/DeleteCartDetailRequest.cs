using e_shop_api.Core.Dto;
using MediatR;

namespace e_shop_api.Applications.Cart.Command.Delete
{
    public class DeleteCartDetailRequest : BaseCommandRequest, IRequest<DeleteCartDetailResponse>
    {
        public string CartDetailId { get; set; }
    }
}
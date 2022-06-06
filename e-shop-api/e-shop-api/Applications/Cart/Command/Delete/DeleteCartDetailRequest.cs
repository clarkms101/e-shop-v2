using MediatR;

namespace e_shop_api.Applications.Cart.Command.Delete
{
    public class DeleteCartDetailRequest : IRequest<DeleteCartDetailResponse>
    {
        public string CartDetailId { get; set; }
    }
}
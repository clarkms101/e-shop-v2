using MediatR;

namespace e_shop_api.Applications.Cart.Command.Update
{
    public class CleanCartRequest : IRequest<CleanCartResponse>
    {
        public string ShoppingCartId { get; set; }
    }
}
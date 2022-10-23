using e_shop_api.Core.Dto;
using MediatR;

namespace EShop.Logic.Applications.Cart.Command.Update
{
    public class CleanCartRequest : BaseCommandRequest, IRequest<CleanCartResponse>
    {
        public string ShoppingCartId { get; set; }
    }
}
using EShop.Cache.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Cart.Command.Update
{
    public class CleanCartHandler : IRequestHandler<CleanCartRequest, CleanCartResponse>
    {
        private readonly IShoppingCartUtility _shoppingCartUtility;
        private readonly ILogger<CleanCartHandler> _logger;

        public CleanCartHandler(IShoppingCartUtility shoppingCartUtility, ILogger<CleanCartHandler> logger)
        {
            _shoppingCartUtility = shoppingCartUtility;
            _logger = logger;
        }

        public async Task<CleanCartResponse> Handle(CleanCartRequest request, CancellationToken cancellationToken)
        {
            _shoppingCartUtility.CleanAllShoppingItemFromCart(request.ShoppingCartId);
            
            return new CleanCartResponse()
            {
                Success = true,
                Message = "購物車已清除!"
            };
        }
    }
}
using EShop.Cache.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Cart.Command.Update
{
    public class CleanCartHandler : IRequestHandler<CleanCartRequest, CleanCartResponse>
    {
        private readonly IShoppingCartCacheUtility _shoppingCartCacheUtility;
        private readonly ILogger<CleanCartHandler> _logger;

        public CleanCartHandler(IShoppingCartCacheUtility shoppingCartCacheUtility, ILogger<CleanCartHandler> logger)
        {
            _shoppingCartCacheUtility = shoppingCartCacheUtility;
            _logger = logger;
        }

        public async Task<CleanCartResponse> Handle(CleanCartRequest request, CancellationToken cancellationToken)
        {
            _shoppingCartCacheUtility.CleanAllShoppingItemFromCart(request.ShoppingCartId);
            
            return new CleanCartResponse()
            {
                Success = true,
                Message = "購物車已清除!"
            };
        }
    }
}
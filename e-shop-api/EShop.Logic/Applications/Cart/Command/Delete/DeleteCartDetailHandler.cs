using e_shop_api.Core.Const;
using EShop.Cache.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Cart.Command.Delete
{
    public class DeleteCartDetailHandler : IRequestHandler<DeleteCartDetailRequest, DeleteCartDetailResponse>
    {
        private readonly IShoppingCartCacheUtility _shoppingCartCacheUtility;
        private readonly ILogger<DeleteCartDetailHandler> _logger;

        public DeleteCartDetailHandler(IShoppingCartCacheUtility shoppingCartCacheUtility,
            ILogger<DeleteCartDetailHandler> logger)
        {
            _shoppingCartCacheUtility = shoppingCartCacheUtility;
            _logger = logger;
        }

        public async Task<DeleteCartDetailResponse> Handle(DeleteCartDetailRequest request,
            CancellationToken cancellationToken)
        {
            var removeIsSuccess =
                _shoppingCartCacheUtility.DeleteShoppingItemFromCart(CartInfo.DefaultCartId, request.CartDetailId);

            if (removeIsSuccess)
            {
                return new DeleteCartDetailResponse()
                {
                    Success = true,
                    Message = "刪除成功!"
                };
            }

            return new DeleteCartDetailResponse()
            {
                Success = false,
                Message = "刪除失敗!"
            };
        }
    }
}
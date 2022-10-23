using e_shop_api.Core.Const;
using EShop.Cache.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Cart.Command.Delete
{
    public class DeleteCartDetailHandler : IRequestHandler<DeleteCartDetailRequest, DeleteCartDetailResponse>
    {
        private readonly IShoppingCartUtility _shoppingCartUtility;
        private readonly ILogger<DeleteCartDetailHandler> _logger;

        public DeleteCartDetailHandler(IShoppingCartUtility shoppingCartUtility,
            ILogger<DeleteCartDetailHandler> logger)
        {
            _shoppingCartUtility = shoppingCartUtility;
            _logger = logger;
        }

        public async Task<DeleteCartDetailResponse> Handle(DeleteCartDetailRequest request,
            CancellationToken cancellationToken)
        {
            var removeIsSuccess =
                _shoppingCartUtility.DeleteShoppingItemFromCart(CartInfo.DefaultCartId, request.CartDetailId);

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
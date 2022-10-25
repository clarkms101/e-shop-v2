using e_shop_api.Core.Const;
using EShop.Cache.Dto;
using EShop.Cache.Interface;
using EShop.Entity.DataBase;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Cart.Command.Create
{
    public class CreateCartDetailHandler : IRequestHandler<CreateCartDetailRequest, CreateCartDetailResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly IShoppingCartCacheUtility _shoppingCartCacheUtility;
        private readonly ILogger<CreateCartDetailHandler> _logger;

        public CreateCartDetailHandler(EShopDbContext eShopDbContext, IShoppingCartCacheUtility shoppingCartCacheUtility,
            ILogger<CreateCartDetailHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _shoppingCartCacheUtility = shoppingCartCacheUtility;
            _logger = logger;
        }

        public async Task<CreateCartDetailResponse> Handle(CreateCartDetailRequest request,
            CancellationToken cancellationToken)
        {
            var product = await _eShopDbContext.Products.FindAsync(request.CartDetail.ProductId);

            if (product == null)
            {
                return new CreateCartDetailResponse()
                {
                    Success = false,
                    Message = "此商品不存在!"
                };
            }

            var addIsSuccess = _shoppingCartCacheUtility.AddShoppingItemToCart(CartInfo.DefaultCartId, new ShoppingItem()
            {
                ProductId = request.CartDetail.ProductId,
                Price = product.Price,
                Qty = request.CartDetail.Qty
            });

            if (addIsSuccess)
            {
                return new CreateCartDetailResponse()
                {
                    Success = true,
                    Message = "加入購物車成功!"
                };
            }

            return new CreateCartDetailResponse()
            {
                Success = false,
                Message = "加入購物車失敗!"
            };
        }
    }
}
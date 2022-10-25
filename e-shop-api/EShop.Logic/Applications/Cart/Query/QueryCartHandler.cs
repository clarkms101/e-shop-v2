using e_shop_api.Core.Const;
using e_shop_api.Core.Dto.Cart;
using e_shop_api.Core.Extensions;
using EShop.Cache.Interface;
using EShop.Entity.DataBase;
using EShop.Logic.Applications.Cart.CommonDto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EShop.Logic.Applications.Cart.Query
{
    public class QueryCartHandler : IRequestHandler<QueryCartRequest, QueryCartResponse>
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly IShoppingCartCacheUtility _shoppingCartCacheUtility;
        private readonly IProductsCacheUtility _productsCacheUtility;
        private readonly ILogger<QueryCartHandler> _logger;

        public QueryCartHandler(EShopDbContext eShopDbContext, IShoppingCartCacheUtility shoppingCartCacheUtility,
            IProductsCacheUtility productsCacheUtility,
            ILogger<QueryCartHandler> logger)
        {
            _eShopDbContext = eShopDbContext;
            _shoppingCartCacheUtility = shoppingCartCacheUtility;
            _productsCacheUtility = productsCacheUtility;
            _logger = logger;
        }

        public async Task<QueryCartResponse> Handle(QueryCartRequest request, CancellationToken cancellationToken)
        {
            // get shopping items from cache
            var shoppingItems = _shoppingCartCacheUtility.GetShoppingItemsFromCart(CartInfo.DefaultCartId);

            if (shoppingItems.Any() == false)
            {
                return new QueryCartResponse()
                {
                    Success = false,
                    Message = "購物車沒有資料!",
                    Carts = new List<CommonDto.Cart>(),
                    TotalAmount = 0,
                    FinalTotalAmount = 0
                };
            }

            // use coupon
            var couponId = _shoppingCartCacheUtility.GetCouponIdFromCart(CartInfo.DefaultCartId);
            var coupon = await _eShopDbContext.Coupons
                .Where(s => s.IsEnabled)
                .Select(s => new ShoppingCoupon()
                {
                    CouponId = s.Id,
                    Title = s.Title,
                    CouponCode = s.CouponCode,
                    Percent = s.Percent,
                    IsEnabled = s.IsEnabled,
                    DueDateTimeStamp = s.DueDateTime.ToTimeStamp()
                }).SingleOrDefaultAsync(s => s.CouponId == couponId, cancellationToken: cancellationToken);

            // 購物車資訊
            var shoppingCarts = new List<CommonDto.Cart>();
            var shoppingTotalAmount = 0m;
            decimal shoppingFinalTotalAmount;
            foreach (var shoppingItem in shoppingItems)
            {
                var productInfo = _productsCacheUtility.GetProductInfo(shoppingItem.ProductId);
                if (productInfo == null)
                {
                    var product = _eShopDbContext.Products.Single(s => s.Id == shoppingItem.ProductId);
                    _productsCacheUtility.AddOrUpdateProductInfo(product);
                    productInfo = _productsCacheUtility.GetProductInfo(shoppingItem.ProductId);
                }

                var cartDetail = new CommonDto.Cart()
                {
                    CartDetailId = shoppingItem.ShoppingItemId,
                    Product = productInfo,
                    // 如果為null，則表示沒有使用coupon
                    Coupon = coupon,
                    Qty = shoppingItem.Qty
                };

                shoppingCarts.Add(cartDetail);
                shoppingTotalAmount += (productInfo.Price * shoppingItem.Qty);
            }

            // 套用優惠券折扣
            if (coupon != null)
            {
                var couponPercent = coupon.Percent / 100f;
                shoppingFinalTotalAmount = shoppingTotalAmount * (decimal)couponPercent;
            }
            else
            {
                shoppingFinalTotalAmount = shoppingTotalAmount;
            }

            return new QueryCartResponse()
            {
                Success = true,
                Message = "查詢成功!",
                Carts = shoppingCarts,
                TotalAmount = shoppingTotalAmount,
                FinalTotalAmount = shoppingFinalTotalAmount
            };
        }
    }
}
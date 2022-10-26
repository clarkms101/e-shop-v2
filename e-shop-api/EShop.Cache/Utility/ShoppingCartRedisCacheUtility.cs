using EShop.Cache.Dto;
using EShop.Cache.Interface;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace EShop.Cache.Utility
{
    public class ShoppingCartRedisCacheUtility : IShoppingCartCacheUtility
    {
        private readonly IConnectionMultiplexer _multiplexer;
        private const string KeyPrefix = "shoppingCart-";

        public ShoppingCartRedisCacheUtility(IConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        private TimeSpan GetExpiryTimeSpan()
        {
            // 3小時後到期
            return new TimeSpan(0, 3, 0, 0);
        }

        public bool AddShoppingItemToCart(string cartId, ShoppingItem shoppingItem)
        {
            var key = $"{KeyPrefix}{cartId}";
            // get cart from cache
            var cartCacheInfoJsonString = _multiplexer.GetDatabase().StringGet(key);

            // Update Cart
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString) == false)
            {
                var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
                var newCartDetail = GetCartDetail(shoppingItem);
                // add item
                cartCacheInfo.ShoppingCartItems.Add(newCartDetail);

                // update cart to cache
                _multiplexer.GetDatabase()
                    .StringSet(key, JsonConvert.SerializeObject(cartCacheInfo), GetExpiryTimeSpan());

                return true;
            }
            // Create Cart
            else
            {
                var newCart = new ShoppingCartCacheInfo()
                {
                    CartId = cartId,
                    CouponId = null,
                    ShoppingCartItems = new List<ShoppingCartItemCache>()
                };
                var newCartDetail = GetCartDetail(shoppingItem);
                // add item
                newCart.ShoppingCartItems.Add(newCartDetail);

                // add cart to cache
                _multiplexer.GetDatabase().StringSet(key, JsonConvert.SerializeObject(newCart), GetExpiryTimeSpan());

                return true;
            }
        }

        public bool DeleteShoppingItemFromCart(string cartId, string shoppingItemId)
        {
            var key = $"{KeyPrefix}{cartId}";
            // get cart from cache
            var cartCacheInfoJsonString = _multiplexer.GetDatabase().StringGet(key);
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString))
            {
                return false;
            }

            var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
            var cartCacheInfoDetail =
                Enumerable.SingleOrDefault<ShoppingCartItemCache>(cartCacheInfo.ShoppingCartItems,
                    s => s.ShoppingItemId == shoppingItemId);
            if (cartCacheInfoDetail == null)
            {
                return false;
            }

            // remove item
            cartCacheInfo.ShoppingCartItems.Remove(cartCacheInfoDetail);

            // reset coupon
            if (cartCacheInfo.ShoppingCartItems.Count == 0)
            {
                cartCacheInfo.CouponId = null;
            }

            // update cart to cache
            _multiplexer.GetDatabase()
                .StringSet(key, JsonConvert.SerializeObject(cartCacheInfo), GetExpiryTimeSpan());

            return true;
        }

        public void CleanAllShoppingItemFromCart(string cartId)
        {
            var key = $"{KeyPrefix}{cartId}";
            _multiplexer.GetDatabase().KeyDelete(key);
        }

        public bool SetCouponIdToCart(string cartId, int couponId)
        {
            var key = $"{KeyPrefix}{cartId}";
            // get cart from cache
            var cartCacheInfoJsonString = _multiplexer.GetDatabase().StringGet(key);
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString))
            {
                return false;
            }

            var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
            cartCacheInfo.CouponId = couponId;

            // update cart to cache
            _multiplexer.GetDatabase()
                .StringSet(key, JsonConvert.SerializeObject(cartCacheInfo), GetExpiryTimeSpan());

            return true;
        }

        public List<ShoppingCartItemCache> GetShoppingItemsFromCart(string cartId)
        {
            var key = $"{KeyPrefix}{cartId}";
            // get cart from cache
            var cartCacheInfoJsonString = _multiplexer.GetDatabase().StringGet(key);

            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString))
            {
                return new List<ShoppingCartItemCache>();
            }

            var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
            return cartCacheInfo.ShoppingCartItems ?? new List<ShoppingCartItemCache>();
        }

        public int? GetCouponIdFromCart(string cartId)
        {
            var key = $"{KeyPrefix}{cartId}";
            // get cart from cache
            var cartCacheInfoJsonString = _multiplexer.GetDatabase().StringGet(key);

            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString))
            {
                return null;
            }

            var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
            return cartCacheInfo.CouponId;
        }

        private static ShoppingCartItemCache GetCartDetail(ShoppingItem shoppingItem)
        {
            var newCartDetail = new ShoppingCartItemCache
            {
                ShoppingItemId = Guid.NewGuid()
                    .ToString("N")
                    .Substring(0, 10),
                ProductId = shoppingItem.ProductId,
                Qty = shoppingItem.Qty,
                Amount = shoppingItem.Price
            };
            return newCartDetail;
        }
    }
}
using System.Runtime.Caching;
using EShop.Cache.Dto;
using EShop.Cache.Interface;
using Newtonsoft.Json;

namespace EShop.Cache.Utility
{
    public class ShoppingCartMemoryCacheUtility : IShoppingCartCacheUtility
    {
        private readonly IMemoryCacheUtility _memoryCacheUtility;
        private const string KeyPrefix = "shoppingCart-";

        public ShoppingCartMemoryCacheUtility(IMemoryCacheUtility memoryCacheUtility)
        {
            _memoryCacheUtility = memoryCacheUtility;
        }

        private CacheItemPolicy GetCacheItemPolicy()
        {
            // 3小時後到期
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(3)
            };
            return cacheItemPolicy;
        }

        public bool AddShoppingItemToCart(string cartId, ShoppingItem shoppingItem)
        {
            var key = $"{KeyPrefix}{cartId}";

            // get cart from cache
            var cartCacheInfoJsonString = _memoryCacheUtility.Get<string>(key);

            // Update Cart
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString) == false)
            {
                var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
                var newCartDetail = GetCartDetail(shoppingItem);
                // add item
                cartCacheInfo.ShoppingCartItems.Add(newCartDetail);

                // update cart to cache
                _memoryCacheUtility.Update(
                    new CacheItem(key, JsonConvert.SerializeObject(cartCacheInfo)),
                    GetCacheItemPolicy());

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
                _memoryCacheUtility.Add(new CacheItem(key, JsonConvert.SerializeObject(newCart)),
                    GetCacheItemPolicy());

                return true;
            }
        }

        public bool DeleteShoppingItemFromCart(string cartId, string shoppingItemId)
        {
            var key = $"{KeyPrefix}{cartId}";

            // get cart from cache
            var cartCacheInfoJsonString = _memoryCacheUtility.Get<string>(key);
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

            // update cart to cache
            _memoryCacheUtility.Update(
                new CacheItem(key, JsonConvert.SerializeObject(cartCacheInfo)),
                GetCacheItemPolicy());

            return true;
        }

        public void CleanAllShoppingItemFromCart(string cartId)
        {
            var key = $"{KeyPrefix}{cartId}";
            _memoryCacheUtility.Remove(key);
        }

        public bool SetCouponIdToCart(string cartId, int couponId)
        {
            var key = $"{KeyPrefix}{cartId}";
            // get cart from cache
            var cartCacheInfoJsonString = _memoryCacheUtility.Get<string>(key);
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString))
            {
                return false;
            }

            var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
            cartCacheInfo.CouponId = couponId;

            // update cart to cache
            _memoryCacheUtility.Update(
                new CacheItem(key, JsonConvert.SerializeObject(cartCacheInfo)),
                GetCacheItemPolicy());
            return true;
        }

        public List<ShoppingCartItemCache> GetShoppingItemsFromCart(string cartId)
        {
            var key = $"{KeyPrefix}{cartId}";
            // get cart from cache
            var cartCacheInfoJsonString = _memoryCacheUtility.Get<string>(key);
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
            var cartCacheInfoJsonString = _memoryCacheUtility.Get<string>(key);
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
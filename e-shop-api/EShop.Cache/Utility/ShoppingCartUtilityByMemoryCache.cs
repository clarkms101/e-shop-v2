using System.Linq;
using System.Runtime.Caching;
using EShop.Cache.Dto;
using EShop.Cache.Interface;
using Newtonsoft.Json;

namespace EShop.Cache.Utility
{
    public class ShoppingCartUtilityByMemoryCache : IShoppingCartUtility
    {
        private readonly IMemoryCacheUtility _memoryCacheUtility;

        public ShoppingCartUtilityByMemoryCache(IMemoryCacheUtility memoryCacheUtility)
        {
            _memoryCacheUtility = memoryCacheUtility;
        }

        public bool AddShoppingItemToCart(string cartId, ShoppingItem shoppingItem)
        {
            // get cart from cache
            var cartCacheInfoJsonString = _memoryCacheUtility.Get<string>(cartId);

            // Update Cart
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString) == false)
            {
                var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
                var newCartDetail = GetCartDetail(shoppingItem);
                // add item
                cartCacheInfo.ShoppingCartItems.Add(newCartDetail);

                // update cart to cache
                _memoryCacheUtility.Update(
                    new CacheItem(cartId, JsonConvert.SerializeObject(cartCacheInfo)),
                    new CacheItemPolicy());

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
                _memoryCacheUtility.Add(new CacheItem(cartId, JsonConvert.SerializeObject(newCart)),
                    new CacheItemPolicy());

                return true;
            }
        }

        public bool DeleteShoppingItemFromCart(string cartId, string shoppingItemId)
        {
            // get cart from cache
            var cartCacheInfoJsonString = _memoryCacheUtility.Get<string>(cartId);
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString))
            {
                return false;
            }

            var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
            var cartCacheInfoDetail =
                Enumerable.SingleOrDefault<ShoppingCartItemCache>(cartCacheInfo.ShoppingCartItems, s => s.ShoppingItemId == shoppingItemId);
            if (cartCacheInfoDetail == null)
            {
                return false;
            }

            // remove item
            cartCacheInfo.ShoppingCartItems.Remove(cartCacheInfoDetail);

            // update cart to cache
            _memoryCacheUtility.Update(
                new CacheItem(cartId, JsonConvert.SerializeObject(cartCacheInfo)),
                new CacheItemPolicy());

            return true;
        }

        public void CleanAllShoppingItemFromCart(string cartId)
        {
            _memoryCacheUtility.Remove(cartId);
        }

        public bool SetCouponIdToCart(string cartId, int couponId)
        {
            // get cart from cache
            var cartCacheInfoJsonString = _memoryCacheUtility.Get<string>(cartId);
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString))
            {
                return false;
            }

            var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
            cartCacheInfo.CouponId = couponId;

            // update cart to cache
            _memoryCacheUtility.Update(
                new CacheItem(cartId, JsonConvert.SerializeObject(cartCacheInfo)),
                new CacheItemPolicy());
            return true;
        }

        public List<ShoppingCartItemCache> GetShoppingItemsFromCart(string cartId)
        {
            // get cart from cache
            var cartCacheInfoJsonString = _memoryCacheUtility.Get<string>(cartId);
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString))
            {
                return new List<ShoppingCartItemCache>();
            }

            var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
            return cartCacheInfo.ShoppingCartItems ?? new List<ShoppingCartItemCache>();
        }

        public int? GetCouponIdFromCart(string cartId)
        {
            // get cart from cache
            var cartCacheInfoJsonString = _memoryCacheUtility.Get<string>(cartId);
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
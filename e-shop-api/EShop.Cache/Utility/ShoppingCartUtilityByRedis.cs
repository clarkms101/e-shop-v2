using System.Linq;
using EShop.Cache.Dto;
using EShop.Cache.Interface;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace EShop.Cache.Utility
{
    public class ShoppingCartUtilityByRedis : IShoppingCartUtility
    {
        private readonly IConnectionMultiplexer _multiplexer;

        public ShoppingCartUtilityByRedis(IConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        public bool AddShoppingItemToCart(string cartId, ShoppingItem shoppingItem)
        {
            // get cart from cache
            var cartCacheInfoJsonString = _multiplexer.GetDatabase().StringGet(cartId);

            // Update Cart
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString) == false)
            {
                var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
                var newCartDetail = GetCartDetail(shoppingItem);
                // add item
                cartCacheInfo.ShoppingCartItems.Add(newCartDetail);

                // update cart to cache
                _multiplexer.GetDatabase().StringSet(cartId, JsonConvert.SerializeObject(cartCacheInfo));

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
                _multiplexer.GetDatabase().StringSet(cartId, JsonConvert.SerializeObject(newCart));

                return true;
            }
        }

        public bool DeleteShoppingItemFromCart(string cartId, string shoppingItemId)
        {
            // get cart from cache
            var cartCacheInfoJsonString = _multiplexer.GetDatabase().StringGet(cartId);
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

            // reset coupon
            if (cartCacheInfo.ShoppingCartItems.Count == 0)
            {
                cartCacheInfo.CouponId = null;
            }

            // update cart to cache
            _multiplexer.GetDatabase().StringSet(cartId, JsonConvert.SerializeObject(cartCacheInfo));

            return true;
        }

        public void CleanAllShoppingItemFromCart(string cartId)
        {
            // _memoryCacheUtility.Remove(cartId);
            _multiplexer.GetDatabase().KeyDelete(cartId);
        }

        public bool SetCouponIdToCart(string cartId, int couponId)
        {
            // get cart from cache
            var cartCacheInfoJsonString = _multiplexer.GetDatabase().StringGet(cartId);
            if (string.IsNullOrWhiteSpace(cartCacheInfoJsonString))
            {
                return false;
            }

            var cartCacheInfo = JsonConvert.DeserializeObject<ShoppingCartCacheInfo>(cartCacheInfoJsonString);
            cartCacheInfo.CouponId = couponId;

            // update cart to cache
            _multiplexer.GetDatabase().StringSet(cartId, JsonConvert.SerializeObject(cartCacheInfo));

            return true;
        }

        public List<ShoppingCartItemCache> GetShoppingItemsFromCart(string cartId)
        {
            // get cart from cache
            var cartCacheInfoJsonString = _multiplexer.GetDatabase().StringGet(cartId);

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
            var cartCacheInfoJsonString = _multiplexer.GetDatabase().StringGet(cartId);

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
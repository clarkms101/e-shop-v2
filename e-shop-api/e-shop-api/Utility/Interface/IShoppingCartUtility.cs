using System.Collections.Generic;
using e_shop_api.Utility.Dto;

namespace e_shop_api.Utility.Interface
{
    public interface IShoppingCartUtility
    {
        bool AddShoppingItemToCart(string cartId, ShoppingItem shoppingItem);
        bool DeleteShoppingItemFromCart(string cartId, string shoppingItemId);
        void CleanAllShoppingItemFromCart(string cartId);
        bool SetCouponIdToCart(string cartId, int couponId);
        List<ShoppingCartItemCache> GetShoppingItemsFromCart(string cartId);
        int? GetCouponIdFromCart(string cartId);
    }
}
using e_shop_api.Core.Dto.Cart;
using EShop.Entity.DataBase.Models;

namespace EShop.Cache.Interface;

public interface IProductsCacheUtility
{
    void AddOrUpdateProductInfo(Product product);
    ShoppingProduct? GetProductInfo(int productId);
}
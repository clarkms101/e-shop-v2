using System.Runtime.Caching;
using e_shop_api.Core.Dto.Cart;
using EShop.Cache.Interface;
using EShop.Entity.DataBase.Models;
using Newtonsoft.Json;

namespace EShop.Cache.Utility;

public class ProductsMemoryCacheUtility : IProductsCacheUtility
{
    private readonly IMemoryCacheUtility _memoryCacheUtility;
    private const string KeyPrefix = "productId-";

    public ProductsMemoryCacheUtility(IMemoryCacheUtility memoryCacheUtility)
    {
        _memoryCacheUtility = memoryCacheUtility;
    }

    public void AddOrUpdateProductInfo(Product product)
    {
        var key = $"{KeyPrefix}{product.Id}";
        var productCacheInfoJsonString = _memoryCacheUtility.Get<string>(key);
        var shoppingProduct = GetShoppingProduct(product);

        if (string.IsNullOrWhiteSpace(productCacheInfoJsonString))
        {
            _memoryCacheUtility.Update(new CacheItem(key, JsonConvert.SerializeObject(shoppingProduct)),
                new CacheItemPolicy());
        }
        else
        {
            _memoryCacheUtility.Add(new CacheItem(key, JsonConvert.SerializeObject(shoppingProduct)),
                new CacheItemPolicy());
        }
    }

    public ShoppingProduct? GetProductInfo(int productId)
    {
        var key = $"{KeyPrefix}{productId}";
        var productCacheInfoJsonString = _memoryCacheUtility.Get<string>(key);

        if (string.IsNullOrWhiteSpace(productCacheInfoJsonString) == false)
        {
            var productCacheInfo = JsonConvert.DeserializeObject<ShoppingProduct>(productCacheInfoJsonString);
            // 轉換為空,回傳空物件
            return productCacheInfo ?? new ShoppingProduct();
        }

        return null;
    }

    private static ShoppingProduct GetShoppingProduct(Product product)
    {
        var shoppingProduct = new ShoppingProduct()
        {
            ProductId = product.Id,
            Title = product.Title,
            Price = product.Price,
            Unit = product.Unit,
            ImageUrl = product.ImageUrl
        };
        return shoppingProduct;
    }
}
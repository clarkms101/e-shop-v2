using System.Runtime.Caching;
using e_shop_api.Core.Dto.Cart;
using EShop.Cache.Interface;
using EShop.Entity.DataBase.Models;
using Newtonsoft.Json;

namespace EShop.Cache.Utility;

public class ProductsMemoryCacheUtility : IProductsCacheUtility
{
    private readonly IBaseMemoryCacheUtility _baseMemoryCacheUtility;
    private const string KeyPrefix = "productId-";

    public ProductsMemoryCacheUtility(IBaseMemoryCacheUtility baseMemoryCacheUtility)
    {
        _baseMemoryCacheUtility = baseMemoryCacheUtility;
    }

    private CacheItemPolicy GetCacheItemPolicy()
    {
        // 6小時後到期
        var cacheItemPolicy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddHours(6)
        };
        return cacheItemPolicy;
    }

    public void AddOrUpdateProductInfo(Product product)
    {
        var key = $"{KeyPrefix}{product.Id}";
        var shoppingProduct = MappingShoppingProduct(product);

        if (string.IsNullOrWhiteSpace(_baseMemoryCacheUtility.Get<string>(key)) == false)
        {
            _baseMemoryCacheUtility.Update(new CacheItem(key, JsonConvert.SerializeObject(shoppingProduct)),
                GetCacheItemPolicy());
        }
        else
        {
            _baseMemoryCacheUtility.Add(new CacheItem(key, JsonConvert.SerializeObject(shoppingProduct)),
                GetCacheItemPolicy());
        }
    }

    public ShoppingProduct? GetProductInfo(int productId)
    {
        var key = $"{KeyPrefix}{productId}";
        var productCacheInfoJsonString = _baseMemoryCacheUtility.Get<string>(key);

        if (string.IsNullOrWhiteSpace(productCacheInfoJsonString) == false)
        {
            var productCacheInfo = JsonConvert.DeserializeObject<ShoppingProduct>(productCacheInfoJsonString);
            // 轉換為空,回傳空物件
            return productCacheInfo ?? new ShoppingProduct();
        }

        return null;
    }

    private static ShoppingProduct MappingShoppingProduct(Product product)
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
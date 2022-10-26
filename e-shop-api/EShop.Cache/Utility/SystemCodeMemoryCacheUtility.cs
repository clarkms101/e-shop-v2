using System.Runtime.Caching;
using e_shop_api.Core.Utility.Dto;
using EShop.Cache.Interface;
using Newtonsoft.Json;

namespace EShop.Cache.Utility;

public class SystemCodeMemoryCacheUtility : ISystemCodeCacheUtility
{
    private readonly IBaseMemoryCacheUtility _baseMemoryCacheUtility;
    private const string KeyPrefix = "systemCodeType-";

    public SystemCodeMemoryCacheUtility(IBaseMemoryCacheUtility baseMemoryCacheUtility)
    {
        _baseMemoryCacheUtility = baseMemoryCacheUtility;
    }

    private CacheItemPolicy GetCacheItemPolicy()
    {
        // 24小時後到期
        var cacheItemPolicy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddHours(24)
        };
        return cacheItemPolicy;
    }

    public void AddSelectionItems(string itemType, List<SelectionItem> items)
    {
        var key = $"{KeyPrefix}{itemType}";
        _baseMemoryCacheUtility.Add(new CacheItem(key, JsonConvert.SerializeObject(items)),
            GetCacheItemPolicy());
    }

    public List<SelectionItem>? GetSelectionItems(string itemType)
    {
        var key = $"{KeyPrefix}{itemType}";
        var selectionJsonString = _baseMemoryCacheUtility.Get<string>(key);

        if (string.IsNullOrWhiteSpace(selectionJsonString) == false)
        {
            var selectionItems = JsonConvert.DeserializeObject<List<SelectionItem>>(selectionJsonString);
            // 轉換為空,回傳空物件
            return selectionItems ?? new List<SelectionItem>();
        }

        return null;
    }
}
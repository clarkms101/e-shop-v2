using System.Runtime.Caching;
using e_shop_api.Core.Utility.Dto;
using EShop.Cache.Interface;
using Newtonsoft.Json;

namespace EShop.Cache.Utility;

public class AdminInfoMemoryCacheUtility : IAdminInfoCacheUtility
{
    private readonly IMemoryCacheUtility _memoryCacheUtility;
    private const string KeyPrefix = "adminInfo-";

    public AdminInfoMemoryCacheUtility(IMemoryCacheUtility memoryCacheUtility)
    {
        _memoryCacheUtility = memoryCacheUtility;
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

    public void AddAdminInfo(string apiAccessKey, AdminInfo adminInfo)
    {
        var key = $"{KeyPrefix}{apiAccessKey}";
        _memoryCacheUtility.Add(new CacheItem(key, JsonConvert.SerializeObject(adminInfo)),
            GetCacheItemPolicy());
    }

    public AdminInfo? GetAdminInfo(string apiAccessKey)
    {
        var key = $"{KeyPrefix}{apiAccessKey}";
        var adminInfoJsonString = _memoryCacheUtility.Get<string>(key);

        if (string.IsNullOrWhiteSpace(adminInfoJsonString) == false)
        {
            var adminInfo = JsonConvert.DeserializeObject<AdminInfo>(adminInfoJsonString);
            // 轉換為空,回傳空物件
            return adminInfo ?? new AdminInfo();
        }

        return null;
    }

    public void RemoveAdminInfo(string apiAccessKey)
    {
        var key = $"{KeyPrefix}{apiAccessKey}";
        _memoryCacheUtility.Remove(key);
    }
}
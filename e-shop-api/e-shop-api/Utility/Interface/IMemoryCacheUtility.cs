using System.Runtime.Caching;

namespace e_shop_api.Utility.Interface
{
    public interface IMemoryCacheUtility
    {
        TReturn Get<TReturn>(string key);
        void Add(CacheItem cacheItem, CacheItemPolicy cacheItemPolicy);
        void Update(CacheItem cacheItem, CacheItemPolicy cacheItemPolicy);
        void Remove(string key);
    }
}
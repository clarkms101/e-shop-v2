using System.Runtime.Caching;

namespace EShop.Cache.Interface
{
    public interface IBaseMemoryCacheUtility
    {
        TReturn Get<TReturn>(string key);
        void Add(CacheItem cacheItem, CacheItemPolicy cacheItemPolicy);
        void Update(CacheItem cacheItem, CacheItemPolicy cacheItemPolicy);
        void Remove(string key);
    }
}
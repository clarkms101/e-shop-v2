using System.Runtime.Caching;
using EShop.Cache.Interface;

namespace EShop.Cache.Utility
{
    public class MemoryCacheUtility : IMemoryCacheUtility
    {
        private readonly MemoryCache _memoryCache;

        public MemoryCacheUtility()
        {
            _memoryCache = MemoryCache.Default;
        }

        public TReturn Get<TReturn>(string key)
        {
            return (TReturn)_memoryCache.Get(key);
        }

        public void Add(CacheItem cacheItem, CacheItemPolicy cacheItemPolicy)
        {
            _memoryCache.Add(cacheItem, cacheItemPolicy);
        }

        public void Update(CacheItem cacheItem, CacheItemPolicy cacheItemPolicy)
        {
            _memoryCache.Set(cacheItem, cacheItemPolicy);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
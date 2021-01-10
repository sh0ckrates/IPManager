using IPManager.WebApi.Data.Abstractions.CacheProvider;
using IPManager.WebApi.Data.Abstractions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace IPManager.WebApi.Data.CacheProvider
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly CacheConfig _config;

        public CacheProvider(IMemoryCache cache, CacheConfig config)
        {
            _cache = cache;
            _config = config;
        }

        public T GetFromCache<T>(string key) where T : class
        {
            var cachedValue = _cache.Get(key);
            return cachedValue as T;
        }

        public void SetCache<T>(string key, T value) where T : class
        {
            var cacheSeconds = _config.Duration;
            _cache.Set(key, value, DateTimeOffset.Now.AddSeconds(cacheSeconds));
        }
    }
}

using Microsoft.Extensions.Caching.Memory;
using MixvelTest.Services.Interfaces;

namespace MixvelTest.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetCachedDataAsync<T>(string cacheKey)
        {
            _memoryCache.TryGetValue(cacheKey, out T data);
            return data;
        }

        public async Task SetCachedDataAsync<T>(
            string cacheKey,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            var options = new MemoryCacheEntryOptions();
            if (absoluteExpireTime.HasValue)
            {
                options.SetAbsoluteExpiration(absoluteExpireTime.Value);
            }
            if (unusedExpireTime.HasValue)
            {
                options.SetSlidingExpiration(unusedExpireTime.Value);
            }

            _memoryCache.Set(cacheKey, data, options);
        }
    }
}

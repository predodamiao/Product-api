using Microsoft.Extensions.Caching.Memory;
using Service.Services.Interfaces;

namespace Service.Services
{
    /// <summary>
    /// Cache Service
    /// </summary>
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// CacheService constructor
        /// </summary>
        /// <param name="memoryCache"></param>
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <inheritdoc/>
        public T? Get<T>(string key)
        {
            return _memoryCache.TryGetValue(key, out T? value) ? value : default(T);
        }

        /// <inheritdoc/>
        public void Set<T>(string key, T value, int expirationInMinutes = 100)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationInMinutes)
            };

            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        /// <inheritdoc/>
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}

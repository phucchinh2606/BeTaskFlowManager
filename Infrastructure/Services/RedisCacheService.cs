using Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var cachedData = await _cache.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(cachedData))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions();
            if (slidingExpiration.HasValue)
            {
                options.SetSlidingExpiration(slidingExpiration.Value);
            }
            else
            {
                options.SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // Mặc định cache 10 phút
            }

            var serializedData = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedData, options, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }
    }
}

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
            try
            {
                var cachedData = await _cache.GetStringAsync(key, cancellationToken);
                if (string.IsNullOrEmpty(cachedData)) return default;
                return JsonSerializer.Deserialize<T>(cachedData);
            }
            catch
            {
                // Nếu Redis lỗi/không kết nối được, coi như Cache Miss để app tiếp tục đọc từ Database
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var options = new DistributedCacheEntryOptions();
                if (slidingExpiration.HasValue)
                    options.SetSlidingExpiration(slidingExpiration.Value);
                else
                    options.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                var serializedData = JsonSerializer.Serialize(value);
                await _cache.SetStringAsync(key, serializedData, options, cancellationToken);
            }
            catch
            {
                // Bỏ qua lỗi ghi cache
            }
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cache.RemoveAsync(key, cancellationToken);
            }
            catch
            {
                // Bỏ qua lỗi xóa cache, không làm gián đoạn Command chính
            }
        }
    }
}

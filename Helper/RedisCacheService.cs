using DistributedCache.Entity;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json;

namespace DistributedCache.Helper
{
    public class RedisCacheService
    {
        private readonly IDatabase _cache;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _cache = connectionMultiplexer.GetDatabase();
        }

        // Set cache with a generic type
        public async Task SetCacheAsync<T>(string key,List<T> value, TimeSpan? expiry = null)
        {
            var serializedValue = JsonHelper.ListToJson(value);
            await _cache.StringSetAsync(key, serializedValue, expiry);
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var serializedValue = JsonHelper.ObjectToJson(value);
            await _cache.StringSetAsync(key, serializedValue, expiry);
        }

        // Get cache with a generic type
        public async Task<List<T?>> GetCacheAsync<T>(string key)
        {
            var value = await _cache.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;

            var list = JsonHelper.JsonToList<T>(value);
            return list;
        }

        // Get cache with a generic type
        public async Task<T?> GetSingleCacheAsync<T>(string key)
        {
            var value = await _cache.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;

            return JsonHelper.JsonToObject<T>(value);
        }

        // Check if cache exists
        public async Task<bool> CacheExistsAsync(string key)
        {
            return await _cache.KeyExistsAsync(key);
        }
    }
}


using DistributedCache.Helper;
using DistributedCache.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Configuration;

namespace DistributedCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController<T>: ControllerBase where T : class
    {

        private readonly RedisCacheService _redisCacheService;
        private readonly IRepository<T> _repository;
        private readonly IConfiguration _configuration;

        public DataController(RedisCacheService redisCacheService, IRepository<T> repository, IConfiguration configuration)
        {
            _redisCacheService = redisCacheService;
            _repository = repository;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetCache")]
        public async Task<IActionResult> Get([FromQuery]string key)
        {
            // Try to get data from Redis cache
            var cachedData = await _redisCacheService.GetCacheAsync<T>(key);

            if (cachedData != null)
            {
                // Return cached data
                return Ok(cachedData);
            }

            // If not in cache, fetch data from database
            var dataFromDb = await _repository.GetDataFromDatabaseAsync<T>(key);

            double time = Convert.ToDouble(_configuration.GetConnectionString("CacheTime"));

            // Store data in cache for future requests
            await _redisCacheService.SetCacheAsync(key, dataFromDb, TimeSpan.FromHours(time));

            return Ok(dataFromDb);
        }

    }
}

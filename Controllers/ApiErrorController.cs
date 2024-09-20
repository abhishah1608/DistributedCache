using DistributedCache.Entity;
using DistributedCache.Helper;
using DistributedCache.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistributedCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiErrorController : DataController<APIErrorDetails>
    {
        public ApiErrorController(RedisCacheService redisCacheService, IRepository<APIErrorDetails> repository, IConfiguration configuration)
            : base(redisCacheService, repository, configuration)
        {

        }
    }
}

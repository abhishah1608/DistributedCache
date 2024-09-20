using api.DapperService;
using DistributedCache.Enum;
using DistributedCache.Helper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DistributedCache.Service
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private readonly SqlConnection _connection;

        public Repository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<T>> GetDataFromDatabaseAsync<T>(string key) where T : class
        {
            List<T> data = default;
            using (DapperService dbservice = new DapperService(_connection))
            {
                string query = QueryBuilderCls.GenerateQuery(key);
                data = dbservice.QueryAsync<T>(query, null).Result.ToList(); 
            }
            return data;
        }
    }
}

namespace DistributedCache.Service
{
    public interface IRepository<T>
    {

        Task<List<T>> GetDataFromDatabaseAsync<T>(string key) where T : class;


    }
}

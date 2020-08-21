using System.Collections.Generic;
using System.Threading.Tasks;

namespace hortus.nosqldb
{
    public interface INoSqlDbService<T>
    {
        Task<IEnumerable<T>> GetItemsAsync(string query);
        Task<T> GetItemAsync(string id, string partitionKey);
        Task AddItemAsync(T item, string partitionKey);
        Task UpdateItemAsync(string id, T item);
        Task DeleteItemAsync(string id);
    }
}
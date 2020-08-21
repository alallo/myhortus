using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace hortus.nosqldb
{

    public class CosmosDbService<T>: INoSqlDbService<T>
    {    
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }
        
        public async Task AddItemAsync(T item, string partitionKey)
        {
            await this._container.CreateItemAsync<T>(item, new PartitionKey(partitionKey));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<T>(id, new PartitionKey(id));
        }

        public async Task<T> GetItemAsync(string id, string partitionKey)
        {
            try
            {
                ItemResponse<T> response = await this._container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch(CosmosException ex)
            { 
                //log
                throw ex;
            }

        }

        public async Task<IEnumerable<T>> GetItemsAsync(string queryString)
        {
            try
            {
                var query = this._container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
                List<T> results = new List<T>();
                while (query.HasMoreResults)
                {
                    var response = await query.ReadNextAsync();
                    
                    results.AddRange(response.ToList());
                }

                return results;
            }
            catch(CosmosException ex)
            { 
                //log
                throw ex;
            }
        }

        public async Task UpdateItemAsync(string id, T item)
        {
            await this._container.UpsertItemAsync<T>(item, new PartitionKey(id));
        }
    }
}

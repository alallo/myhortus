using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using hortus.nosqldb;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using hortus.entities;
using hortus.storage;

[assembly: FunctionsStartup(typeof(hortus.functions.Startup))]

namespace hortus.functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var appSettings = new AppConfiguration().GetAppSettings();
            builder.Services.AddSingleton<AppSettings>(appSettings);
            builder.Services.AddSingleton<INoSqlDbService<GardenStory>>(InitializeCosmosClientInstanceAsync(appSettings).GetAwaiter().GetResult());
            builder.Services.AddSingleton<IImagesStorage>(InitializeStorageClientInstance(appSettings));

        }

        private static async Task<CosmosDbService<GardenStory>> InitializeCosmosClientInstanceAsync(AppSettings appSettings)
        {
            CosmosClient client = new CosmosClient(appSettings.CosmosDbConnectionString);
            CosmosDbService<GardenStory> cosmosDbService = new CosmosDbService<GardenStory>(client, appSettings.CosmosDbName, appSettings.CosmosDbContainerName);
            DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(appSettings.CosmosDbName);
            await database.Database.CreateContainerIfNotExistsAsync(appSettings.CosmosDbContainerName, appSettings.CosmosDbPartitionKey);

            return cosmosDbService;
        }

        private static ImagesStorage InitializeStorageClientInstance(AppSettings appSettings)
        {
            var imagesStorage = new ImagesStorage(appSettings.StorageConnectionString, appSettings.StorageImagesContainerName);
            return imagesStorage;
        }
    }
}
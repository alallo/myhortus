using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using hortus.nosqldb;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using hortus.entities;

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

        }

        private static async Task<CosmosDbService<GardenStory>> InitializeCosmosClientInstanceAsync(AppSettings appSettings)
        {
            CosmosClient client = new CosmosClient(appSettings.CosmosDbConnectionString);
            CosmosDbService<GardenStory> cosmosDbService = new CosmosDbService<GardenStory>(client, appSettings.CosmosDbName, appSettings.ContainerName);
            Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(appSettings.CosmosDbName);
            await database.Database.CreateContainerIfNotExistsAsync(appSettings.ContainerName, "/postcode");

            return cosmosDbService;
        }
    }
}
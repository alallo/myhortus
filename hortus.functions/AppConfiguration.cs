using System.IO;
using Microsoft.Extensions.Configuration;

namespace hortus.functions
{
    public class AppConfiguration
    {
        public AppSettings GetAppSettings()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
                var settings = new AppSettings();
                settings.CosmosDbConnectionString= config["CosmosDbConnectionString"];
                settings.CosmosDbName = config["CosmosDbName"];
                settings.CosmosDbContainerName = config["CosmosContainerName"];
                settings.StorageConnectionString = config["AzureWebJobsStorage"];
                settings.StorageImagesContainerName = config["StorageContainerName"];
                settings.CosmosDbPartitionKey = config["CosmosDbPartitionKey"];

                return settings;
        }
    }
}
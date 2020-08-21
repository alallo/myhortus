namespace hortus.functions
{
    public class AppSettings
    {
        public string CosmosDbConnectionString{ get; set; }

        public string CosmosDbName { get; set; }

        public string CosmosDbContainerName { get; set; }

        public string StorageConnectionString { get; set; }

        public string StorageImagesContainerName { get; set; }

        public string CosmosDbPartitionKey { get; set; }
    }
}
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using hortus.nosqldb;
using hortus.entities;

namespace hortus.functions
{
    public class UploadGardenStory
    {
        private AppSettings appSettings {get; set;}
        private INoSqlDbService<GardenStory> dbService {get; set;}

        public UploadGardenStory(AppSettings settings, INoSqlDbService<GardenStory> db)
        {
            appSettings = settings;
            dbService = db;
        }

        [FunctionName("UploadGardenStory")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            GardenStory data = JsonConvert.DeserializeObject<GardenStory>(requestBody);
            data.Id = Guid.NewGuid().ToString();
            await dbService.AddItemAsync(data, data.PostCode);

            return new OkResult();
        }
    }
}

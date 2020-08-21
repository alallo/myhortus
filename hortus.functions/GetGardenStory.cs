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
    public class GetGardenStory
    {
        private INoSqlDbService<GardenStory> dbService {get; set;}

        public GetGardenStory(INoSqlDbService<GardenStory> db)
        {
            dbService = db;
        }

        [FunctionName("GetGardenStory")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "hortus/story/get/{postcode}/{id}")] HttpRequest req, string postcode, string id,
            ILogger log)
        {
            try
            {
                log.LogInformation("GetGardenStory trigger function processed a request.");
                var story = await dbService.GetItemAsync(id, postcode);
                 return new OkObjectResult(story);
            }
            catch(Exception) 
            {
                return new NotFoundResult();
            }
        }
    }
}

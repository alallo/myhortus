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
    public class GetNeighboursGardenStories
    {

        private INoSqlDbService<GardenStory> dbService {get; set;}

        public GetNeighboursGardenStories(INoSqlDbService<GardenStory> db)
        {
            dbService = db;
        }

        [FunctionName("GetNeighboursGardenStories")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "hortus/story/neighbours/{postcode}")] HttpRequest req, string postcode,
            ILogger log)
        {
            log.LogInformation("GetNeighboursGardenStories trigger function processed a request.");

            var query = $"SELECT * FROM c where c.postcode = '{postcode}'";
            var stories = await dbService.GetItemsAsync(query);
            return new OkObjectResult(stories);

        }
    }
}

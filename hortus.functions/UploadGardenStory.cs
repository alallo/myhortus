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
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "hortus/story/upload/")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("UploadGardenStory trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var story = JsonConvert.DeserializeObject<GardenStory>(requestBody);
            if(string.IsNullOrEmpty(story.Id))
            {           
                story.Id = Guid.NewGuid().ToString();
                story.DateCreated = DateTime.Now;
                story.LastUpdated = DateTime.Now;
                await dbService.AddItemAsync(story, story.PostCode);
                return new OkObjectResult(story.Id);
            }
            else
            {
                story.LastUpdated = DateTime.Now;
                await dbService.UpdateItemAsync(story.PostCode, story);
                return new OkResult();
            }
        }
    }
}
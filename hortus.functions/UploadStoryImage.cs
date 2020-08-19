using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using hortus.storage;

namespace hortus.functions
{
    public class UploadStoryImage
    {
        private AppSettings appSettings {get; set;}

        private IImagesStorage imageStorage {get; set;}

        public UploadStoryImage(AppSettings settings, IImagesStorage storage)
        {
            appSettings = settings;
            imageStorage = storage;
        }

        [FunctionName("UploadStoryImage")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "hortus/story/uploadimage/{id}")] HttpRequestMessage req, string id,
            ILogger log)
        {
            var provider = new MultipartMemoryStreamProvider();
            await req.Content.ReadAsMultipartAsync(provider);
            var file = provider.Contents[0];
            var fileInfo = file.Headers.ContentDisposition;
            var fileData = await file.ReadAsByteArrayAsync();
            var content = req.Content;
                string jsonContent = content.ReadAsStringAsync().Result;
            string uri = await imageStorage.UploadFileToBlobAsync(fileInfo.FileName, fileData, id);
            if(string.IsNullOrEmpty(uri))
                return new BadRequestResult();
            return new OkObjectResult(uri);
        }
    }
}

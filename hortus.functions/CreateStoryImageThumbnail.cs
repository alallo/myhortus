using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace hortus.functions
{
    public class CreateStoryImageThumbnail
    {
        [FunctionName("CreateStoryImageThumbnail")]
        public async void Run([BlobTrigger("storyImages/{name}", Connection = "StorageConnection")]Stream streamImage, string name,
            [Blob("thumbs/s-{name}", FileAccess.Write, Connection = "StorageConnection")]CloudBlobContainer outputContainer,
            ILogger log)
        {
            Image image = Image.FromStream(streamImage);
            Image thumb = image.GetThumbnailImage(150, 150, () => false, IntPtr.Zero);
            var ms = new MemoryStream();
            thumb.Save(ms, ImageFormat.Jpeg);
            ms.Position = 0;

            await outputContainer.CreateIfNotExistsAsync();
  
            var cloudBlockBlob = outputContainer.GetBlockBlobReference(name);
            await cloudBlockBlob.UploadFromStreamAsync(ms);
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {streamImage.Length} Bytes");
        }
    }
}

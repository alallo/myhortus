using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;


namespace hortus.storage
{
    public class ImagesStorage: IImagesStorage
    {
        private BlobServiceClient blobServiceClient { get; set; }
        private BlobContainerClient blobContainerClient { get; set; }

        public ImagesStorage(string connectionString, string containerName)
        {
            blobServiceClient = new BlobServiceClient(connectionString);
            blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            blobContainerClient.CreateIfNotExists();
        }
  
        public async Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string id)  
        {  
            try  
            {  
                string fileName = this.GenerateFileName(strFileName, id);  
  
                if (fileName != null && fileData != null)  
                {  

                    using(var stream = new MemoryStream(fileData, 0 , fileData.Length))
                    {
                        BlobClient blob = blobContainerClient.GetBlobClient(fileName);
                        await blob.UploadAsync(stream);
                        var imageUrlPath = blob.Uri.AbsoluteUri;

                        return imageUrlPath; 
                    }
                }

                return "";
            }  
            catch (Exception ex)  
            {  
                throw (ex);  
            }  
        }  

        private string GenerateFileName(string fileName, string id)  
        {  
            string strFileName = string.Empty;  
            string[] strName = fileName.Split('.');  
            strFileName = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff") + "_" 
            + id + "." + strName[strName.Length - 1].Replace("\"", "");  
            return strFileName;  
        }  
    }
}

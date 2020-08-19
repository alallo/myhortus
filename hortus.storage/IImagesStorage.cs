using System.Threading.Tasks;

namespace hortus.storage
{
    public interface IImagesStorage
    {
        public Task<string> UploadFileToBlobAsync(string strFileName, byte[] fileData, string id);
    }
}
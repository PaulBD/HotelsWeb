using System.IO;

namespace library.azure.services
{
    public interface IStorageService
    {
        void UploadToStorage(string containerName, Stream fileStream, string fileName, string contentType);
    }
}

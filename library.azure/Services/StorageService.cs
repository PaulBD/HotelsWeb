using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace library.azure.services
{
    public class StorageService : IStorageService
	{
		private const string AccountName = "triperoostorage";
		private const string AccountKey = "M4sPK7yu8zkDYho4kiel1PGDeDPOxZkJ071aIrmdWLcTo2CMsf0fXSg6+X5AHtilo8EFiMtr0HqiWgsdDb1ndw==";

		/// <summary>
		/// Upload to Azure
		/// </summary>
		public void UploadToStorage(string containerName, Stream fileStream, string fileName, string contentType)
		{
			var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=" + AccountName + ";AccountKey=" + AccountKey + ";EndpointSuffix=core.windows.net");

			var blobClient = storageAccount.CreateCloudBlobClient();

			var container = blobClient.GetContainerReference(containerName);
			container.CreateIfNotExists();

			container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

			var blockBlob = container.GetBlockBlobReference(fileName);
			blockBlob.Properties.ContentType = contentType;
			blockBlob.UploadFromStream(fileStream);
		}
    }
}

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Event.Application.Interfaces;
using System.Security.Policy;

namespace Event.Infastructure.Implementations
{
    public class BlobService : IBlobService
    {
        private const string BLOB_FOLDER = "images";
        private readonly BlobContainerClient blobContainerClient;

        public BlobService(
            BlobServiceClient blobServiceClient)
        {
            blobContainerClient = blobServiceClient.GetBlobContainerClient(BLOB_FOLDER);
            blobContainerClient.CreateIfNotExists();
            blobContainerClient.SetAccessPolicy(PublicAccessType.Blob);
        }

        public async Task DeleteBlobFolder(string blobFolder, 
            CancellationToken cancellationToken = default)
        {
            var blobClient = blobContainerClient.GetBlobClient(blobFolder);

            await blobContainerClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public async Task<string> DownLoadBlob(string blobName, 
            CancellationToken cancellationToken = default)
        {
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync(cancellationToken))
            { 
                return $"http://localhost:10000/faith725/{BLOB_FOLDER}/{blobClient.Name}";
            }

            return "";
        }

        public async Task<IEnumerable<string>> DownloadBlobs(string folderName, 
            CancellationToken cancellationToken = default)
        {
            var urls = new List<string>();  

            var blobs = blobContainerClient.GetBlobsAsync(prefix: folderName,
                cancellationToken: cancellationToken);

            await foreach(var blob in blobs)
            {
                var blobClient = blobContainerClient.GetBlobClient(blob.Name);

                var blobUrl = $"http://localhost:10000/faith725/{BLOB_FOLDER}/{blobClient.Name}";

                urls.Add(blobUrl);
            }

            return urls;
        }

        public async Task<string> UploadBlob(Stream stream, 
            string blobName,
            string contentType, 
            string folder = "",
            CancellationToken cancellationToken = default)
        {
            var blobPath =  folder + "/" +Guid.NewGuid().ToString() + "-" + blobName ;

            var blobClient = blobContainerClient.GetBlobClient(blobPath);

            await blobClient.UploadAsync(stream,
                new BlobHttpHeaders { ContentType = contentType},
                cancellationToken: cancellationToken);

            var blobUrl = $"http://localhost:10000/faith725/{BLOB_FOLDER}/{blobClient.Name}";

            return blobUrl;
        }
    }
}

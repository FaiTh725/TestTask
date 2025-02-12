using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Event.Application.Interfaces;

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
                return blobClient.Uri.ToString();
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
                urls.Add(blobClient.Uri.AbsoluteUri.ToString());
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
            
            return blobClient.Uri.AbsoluteUri.ToString();
        }
    }
}

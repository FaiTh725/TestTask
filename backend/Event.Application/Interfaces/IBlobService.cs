namespace Event.Application.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadBlob(
            Stream stream,
            string blobName,
            string contentType, 
            string folder = "",
            CancellationToken cancellationToken = default);

        Task<string> DownLoadBlob(string blobName, 
            CancellationToken cancellationToken = default);

        Task<IEnumerable<string>> DownloadBlobs(string folderName, 
            CancellationToken cancellationToken = default);

        Task DeleteBlobFolder(string blobFolder,
            CancellationToken cancellationToken = default);
    }
}

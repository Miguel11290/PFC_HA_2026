using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace MenuWebApp.Services
{
    public class StorageService: IStorageService
    {
        private readonly StorageClient _storageClient;
        private readonly GcpOptions _gcpOptions;

        public StorageService(IOptions<GcpOptions> gcpOptions)
        {
            _storageClient = StorageClient.Create();
            _gcpOptions = gcpOptions.Value;
        }

        public async Task<(string BucketName, string ObjectPath)> UploadMenuImageAsync(
            IFormFile file,
            string restaurantId,
            CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("No file was provided.");

            var safeFileName = Path.GetFileName(file.FileName);
            var extension = Path.GetExtension(safeFileName);
            var objectName = $"restaurants/{restaurantId}/{DateTime.UtcNow:yyyy/MM/dd}/{Guid.NewGuid()}{extension}";

            await using var stream = file.OpenReadStream();

            await _storageClient.UploadObjectAsync(
                bucket: _gcpOptions.BucketName,
                objectName: objectName,
                contentType: file.ContentType,
                source: stream,
                options: null,
                cancellationToken: cancellationToken);

            return (_gcpOptions.BucketName, objectName);
        }
    }
}

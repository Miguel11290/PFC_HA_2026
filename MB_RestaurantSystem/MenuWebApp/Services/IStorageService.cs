using Microsoft.AspNetCore.Http;

namespace MenuWebApp.Services
{
    public interface IStorageService
    {
        Task<(string BucketName, string ObjectPath)> UploadMenuImageAsync(IFormFile file, string restaurantId, CancellationToken cancellationToken = default);
    }
}

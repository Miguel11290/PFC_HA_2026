namespace MenuWebApp.Services
{
    public interface IPubSubService
    {
        Task PublishMenuUploadAsync(
            string restaurantId,
            string menuId,
            string imageId,
            string bucket,
            string objectPath,
            string uploadedBy,
            CancellationToken cancellationToken = default);
    }
}

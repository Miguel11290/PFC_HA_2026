using Shared.Models;

namespace MenuWebApp.Services
{
    public interface IFirestoreService
    {
        Task EnsureRestaurantExistsAsync(string restaurantId, string restaurantName, CancellationToken cancellationToken = default);
        Task EnsureMenuExistsAsync(string restaurantId, string menuId, CancellationToken cancellationToken = default);

        Task<string> CreateImageReferenceAsync(
            string restaurantId,
            string menuId,
            MenuImageDocument imageDocument,
            CancellationToken cancellationToken = default);
    }
}

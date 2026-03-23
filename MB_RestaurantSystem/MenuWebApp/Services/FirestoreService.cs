using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using Shared.Models;
using Shared.Options;

namespace MenuWebApp.Services
{
    public class FirestoreService : IFirestoreService
    {
        private readonly FirestoreDb _firestoreDB;

        public FirestoreService(IOptions<GcpOptions> gcpOptions)
        {
            _firestoreDB = FirestoreDb.Create(gcpOptions.Value.ProjectId);
        }

        public async Task EnsureRestaurantExistsAsync(string restaurantId, string restaurantName, CancellationToken cancellationToken = default)
        {
            var restaurantRef = _firestoreDB.Collection("restaurants").Document(restaurantId);
            var snapshot = await restaurantRef.GetSnapshotAsync(cancellationToken);
            if (!snapshot.Exists)
            {
                var restaurant = new Restaurant
                {
                    Id = restaurantId,
                    Name = restaurantName,
                    Status = "pending",
                    UpdatedAtUtc = DateTime.UtcNow
                };

                await restaurantRef.SetAsync(restaurant, cancellationToken: cancellationToken);
            }
        }

        public async Task EnsureMenuExistsAsync(string restaurantId, string menuId, CancellationToken cancellationToken = default)
        {
            var menuRef = _firestoreDB.Collection("restaurants").Document(restaurantId).Collection("menus").Document(menuId);
            var snapshot = await menuRef.GetSnapshotAsync(cancellationToken);

            if (!snapshot.Exists)
            {
                var menu = new Dictionary<string, object>
                {
                    { "id", menuId },
                    { "status", "pending" },
                    { "ocrText", "" },
                    { "createdAtUtc", DateTime.UtcNow },
                    { "updatedAtUtc", DateTime.UtcNow }
                };

                await menuRef.SetAsync(menu, cancellationToken: cancellationToken);
            }
        }

        public async Task<string> CreateImageReferenceAsync(string restaurantId, string menuId, MenuImageDocument imageDocument, CancellationToken cancellationToken = default)
        {
            var imagesCollection = _firestoreDB.Collection("restaurants").Document(restaurantId).Collection("menus").Document(menuId).Collection("images");
            var imageRef = imagesCollection.Document();

            imageDocument.Id = imageRef.Id;
            imageDocument.UploadedAtUtc = DateTime.UtcNow;

            await imageRef.SetAsync(imageDocument, cancellationToken: cancellationToken);

            return imageRef.Id;
        }
    }
}

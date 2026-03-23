using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace OcrProcessor.Services
{
    public class FirestoreUpdateService
    {
        private readonly FirestoreDb _firestoreDB;

        public FirestoreUpdateService(IOptions<GcpOptions> gcpOptions)
        {
            _firestoreDB = FirestoreDb.Create(gcpOptions.Value.ProjectId);
        }

        public async Task SaveOcrResultAsync(string restaurantId, string menuId, string imageId, string ocrText, CancellationToken cancellationToken = default)
        {
            var menuRef = _firestoreDB.Collection("restaurants").Document(restaurantId).Collection("menus").Document(menuId);
            var imageRef = menuRef.Collection("images").Document(imageId);

            await imageRef.UpdateAsync(new Dictionary<string, object>
            {
                {"VisionProcessed", true },
                {"RawVisionText", ocrText }
            }, cancellationToken: cancellationToken);

            await menuRef.UpdateAsync(new Dictionary<string, object>
            {
                {"ocrText", ocrText },
                {"status", "processed" },
                {"updatedAtUtc", DateTime.UtcNow }
            }, cancellationToken: cancellationToken);

            var restaurantRef = _firestoreDB.Collection("restaurants").Document(restaurantId);

            await restaurantRef.UpdateAsync(new Dictionary<string, object>
            {
                {"Status", "processed" },
                {"UpdatedAtUtc", DateTime.UtcNow }
            }, cancellationToken: cancellationToken);
        }
    }
}

using Google.Apis.Storage.v1.Data;
using MenuWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace MenuWebApp.Controllers
{
    public class TestCloudController : Controller
    {
        private readonly IFirestoreService _firestoreService;
        private readonly IPubSubService _pubSubService;

        public TestCloudController(IFirestoreService firestoreService, IPubSubService pubSubService)
        {
            _firestoreService = firestoreService;
            _pubSubService = pubSubService;
        }

        [HttpGet("/test-cloud")]
        public async Task<IActionResult> Index()
        {
            var restaurantId = "test-restaurant";
            var menuId = "test-menu";

            await _firestoreService.EnsureRestaurantExistsAsync(restaurantId, "Test Restaurant");

            var imageId = await _firestoreService.CreateImageReferenceAsync(restaurantId, menuId, new MenuImageDocument
            {
                Bucket = "test-bucket",
                ObjectPath = "test/path/image.jpg",
                FileName = "image.jpg",
                UploadedBy = "local@test.com",
                VisionProcessed = false,
                RawVisionText = ""
            });

            await _pubSubService.PublishMenuUploadAsync(restaurantId, menuId, imageId, "test-bucket", "test/path/image.jpg", "local@test.com");
            return Content("Cloud test completed successfully.");
        }
    }
}

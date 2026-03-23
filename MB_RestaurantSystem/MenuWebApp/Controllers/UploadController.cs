using MenuWebApp.Models;
using MenuWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace MenuWebApp.Controllers;

public class UploadController : Controller
{
    private readonly IStorageService _storageService;
    private readonly IFirestoreService _firestoreService;
    private readonly IPubSubService _pubSubService;

    public UploadController(
        IStorageService storageService,
        IFirestoreService firestoreService,
        IPubSubService pubSubService)
    {
        _storageService = storageService;
        _firestoreService = firestoreService;
        _pubSubService = pubSubService;
    }

    [HttpGet("/Upload")]
    public IActionResult Index()
    {
        return View(new UploadMenuViewModel());
    }

    [HttpPost("/api/upload-menu-file")]
    [RequestSizeLimit(50_000_000)]
    public async Task<IActionResult> UploadMenuFile(
    [FromForm] SingleFileUploadRequest model,
    CancellationToken cancellationToken)
    {
        var response = new UploadResponseViewModel();

        if (!ModelState.IsValid)
        {
            response.Success = false;
            response.Message = "Validation failed.";
            response.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(response);
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(model.File.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
        {
            response.Success = false;
            response.Message = $"File type not allowed: {model.File.FileName}";
            return BadRequest(response);
        }

        try
        {
            var restaurantId = model.RestaurantId.Trim();
            var restaurantName = model.RestaurantName.Trim();
            var menuId = model.MenuId.Trim();

            await _firestoreService.EnsureRestaurantExistsAsync(
                restaurantId,
                restaurantName,
                cancellationToken);

            await _firestoreService.EnsureMenuExistsAsync(
                restaurantId,
                menuId,
                cancellationToken);

            var storageResult = await _storageService.UploadMenuImageAsync(
                model.File,
                restaurantId,
                cancellationToken);

            var imageDocument = new MenuImageDocument
            {
                Bucket = storageResult.BucketName,
                ObjectPath = storageResult.ObjectPath,
                FileName = model.File.FileName,
                UploadedBy = User?.Identity?.Name ?? "local-user",
                VisionProcessed = false,
                RawVisionText = string.Empty
            };

            var imageId = await _firestoreService.CreateImageReferenceAsync(
                restaurantId,
                menuId,
                imageDocument,
                cancellationToken);

            await _pubSubService.PublishMenuUploadAsync(
                restaurantId,
                menuId,
                imageId,
                imageDocument.Bucket,
                imageDocument.ObjectPath,
                imageDocument.UploadedBy,
                cancellationToken);

            response.Success = true;
            response.Message = "File uploaded successfully and queued for OCR processing.";
            response.UploadedFiles.Add(model.File.FileName);

            return Json(response);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "Upload failed.";
            response.Errors.Add(ex.Message);

            return StatusCode(500, response);
        }
    }
}
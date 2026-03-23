using OcrProcessor.Models;
using OcrProcessor.Services;
using Shared.Options;
using System.Text;
using System.Text.Json;

namespace OcrProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {         
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<GcpOptions>(
                builder.Configuration.GetSection("Gcp"));

            builder.Services.AddSingleton<VisionOcrService>();
            builder.Services.AddScoped<FirestoreUpdateService>();

            var app = builder.Build();

            app.MapGet("/", () => "OcrProcessor is running.");

            app.MapPost("/", async (
                PubSubPushEnvelope envelope,
                VisionOcrService visionOcrService,
                FirestoreUpdateService firestoreUpdateService,
                ILogger<Program> logger,
                CancellationToken cancellationToken) =>
            {
                if (envelope?.Message?.Data == null)
                {
                    logger.LogWarning("No Pub/Sub message data received.");
                    return Results.BadRequest("Missing message data.");
                }

                try
                {
                    var json = Encoding.UTF8.GetString(
                        Convert.FromBase64String(envelope.Message.Data));

                    var menuUploadEvent = JsonSerializer.Deserialize<MenuUploadEvent>(json);

                    if (menuUploadEvent == null)
                    {
                        logger.LogWarning("Failed to deserialize menu upload event.");
                        return Results.BadRequest("Invalid event payload.");
                    }

                    var gcsUri = $"gs://{menuUploadEvent.Bucket}/{menuUploadEvent.ObjectPath}";
                    logger.LogInformation("Processing OCR for {GcsUri}", gcsUri);

                    var ocrText = await visionOcrService.ExtractTextFromGcsImageAsync(
                        gcsUri,
                        cancellationToken);

                    await firestoreUpdateService.SaveOcrResultAsync(
                        menuUploadEvent.RestaurantId,
                        menuUploadEvent.MenuId,
                        menuUploadEvent.ImageId,
                        ocrText,
                        cancellationToken);

                    logger.LogInformation(
                        "OCR processed successfully for restaurant {RestaurantId}, menu {MenuId}, image {ImageId}",
                        menuUploadEvent.RestaurantId,
                        menuUploadEvent.MenuId,
                        menuUploadEvent.ImageId);

                    return Results.Ok(new { success = true });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "OCR processing failed.");
                    return Results.Problem("OCR processing failed.");
                }
            });

            app.Run();
        }
    }
}

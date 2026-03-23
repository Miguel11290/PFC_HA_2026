using Google.Cloud.Vision.V1;

namespace OcrProcessor.Services
{
    public class VisionOcrService
    {
        private readonly ImageAnnotatorClient _visionClient;

        public VisionOcrService()
        {
            _visionClient = ImageAnnotatorClient.Create();
        }

        public async Task<string> ExtractTextFromGcsImageAsync(string gcsUri, CancellationToken cancellationToken = default)
        {
            var image = Image.FromUri(gcsUri);
            var response = await _visionClient.DetectDocumentTextAsync(image);
            return response?.Text ?? string.Empty;
        }
    }
}

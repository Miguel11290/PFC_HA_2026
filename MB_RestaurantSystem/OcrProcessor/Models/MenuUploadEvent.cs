namespace OcrProcessor.Models
{
    public class MenuUploadEvent
    {
        public string RestaurantId { get; set; } = string.Empty;
        public string MenuId { get; set; } = string.Empty;
        public string ImageId { get; set; } = string.Empty;
        public string Bucket { get; set; } = string.Empty;
        public string ObjectPath { get; set; } = string.Empty;
        public string UploadedBy { get; set; } = string.Empty;
    }
}

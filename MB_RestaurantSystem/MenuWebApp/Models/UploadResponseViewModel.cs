namespace MenuWebApp.Models
{
    public class UploadResponseViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> UploadedFiles { get; set; } = new();
        public List<string> Errors { get; set; } = new();
    }
}

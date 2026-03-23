using System.ComponentModel.DataAnnotations;

namespace MenuWebApp.Models
{
    public class SingleFileUploadRequest
    {
        [Required]
        public string RestaurantId { get; set; } = string.Empty;

        [Required]
        public string RestaurantName { get; set; } = string.Empty;

        [Required]
        public string MenuId { get; set; } = string.Empty;

        [Required]
        public IFormFile File { get; set; } = default!;
    }
}

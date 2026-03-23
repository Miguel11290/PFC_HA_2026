using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MenuWebApp.Models
{
    public class UploadMenuViewModel
    {
        [Required]
        public string RestaurantId { get; set; } = string.Empty;

        [Required]
        public string RestaurantName { get; set; } = string.Empty;

        [Required]
        public List<IFormFile> Files { get; set; } = new();
    }
}


using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Helpers
{
    public class FileUpload
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile? FormFile { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public byte[]? FileByteArray { get; set; }

        public string EntityId { get; set; }  = null!;
        public string EntityType { get; set; } = null!;
    }
}

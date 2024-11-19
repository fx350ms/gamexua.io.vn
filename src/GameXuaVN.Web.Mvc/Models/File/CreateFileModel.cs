using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GameXuaVN.Web.Models.File
{
    public class CreateFileModel
    {
        [Required]
        public string FileName { get; set; }

        public string Description { get; set; }

        public int DownloadCount { get; set; }
        public int TotalRateCount { get; set; }
        public float TotalRate { get; set; }
        public string DownloadUrl { get; set; }
        // Kiểu MIME của file ảnh (ví dụ: image/jpeg, image/png)
        public string ContentType { get; set; }

        public int CategoryId { get; set; }

        public IFormFile FileData { get; set; } // File upload từ người dùng
        public IFormFile Thumbnail { get; set; } // Thumbnail nếu có
        
    }
}

using Abp.AutoMapper;
using GameXuaVN.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GameXuaVN.Games.Dto
{

    [AutoMapFrom(typeof(Game))]
    public class CreateGameDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }
        public int TotalPlay { get; set; }
        public int TotalLike { get; set; }
        public int TotalDislike { get; set; }

        public IFormFile ThumbnailFromFile { get; set; }

        public byte[] Thumbnail { get; set; }

        public IFormFile DataFromFile { get; set; }

        // Dữ liệu ảnh (binary)
        public byte[] Data { get; set; }

        // Kiểu MIME của file ảnh (ví dụ: image/jpeg, image/png)
        public string ContentType { get; set; }

        public int CategoryId { get; set; }
        public string EmbedUrl { get; set; }
        [MaxLength(2)]
        public string Page { get; set; } // #,A,B,C,D

    }
}

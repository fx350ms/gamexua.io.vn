using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using GameXuaVN.Validation;

namespace GameXuaVN.Web.Models.Games
{
    public class CreateOrUpdateGameModel 
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalPlay { get; set; }
        public int TotalLike { get; set; }
        public int TotalDislike { get; set; }
        public byte[] Thumbnail { get; set; }

        // Dữ liệu ảnh (binary)
        public byte[] Data { get; set; }

        // Kiểu MIME của file ảnh (ví dụ: image/jpeg, image/png)
        public string ContentType { get; set; }

        public int CategoryId { get; set; }

        public string EmbedUrl { get; set; }

        [MaxLength(2)]

        public string Page { get; set; }
    }
}

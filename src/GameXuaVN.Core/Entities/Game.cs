using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GameXuaVN.Entities
{
    public class Game : FullAuditedEntity<int>
    {
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

        public string Page {  get; set; } // #,A,B,C,D

    }
}

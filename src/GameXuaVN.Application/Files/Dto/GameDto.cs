using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using GameXuaVN.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace GameXuaVN.Games.Dto
{
    [AutoMapFrom(typeof(Game))]
    public class GameDto :  EntityDto<int>
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


        public string Page { get; set; } // #,A,B,C,D

        public string ThumbnailBase64 => $"data:image/jpeg;base64, {Convert.ToBase64String(Thumbnail)}";

        public string Title => Name.Replace(" ", "");

        public string Url => Name.Replace(" ", "-");

    }
}

using Abp.Application.Services.Dto;
using System;

namespace GameXuaVN.Games.Dto
{
    //custom PagedResultRequestDto
    public class PagedGameResultRequestDto : PagedResultRequestDto
    {
        public string Page { get; set; } = "";
        public int CategoryId { get; set; } = -1;
        public string Keyword { get; set; }
    }
}

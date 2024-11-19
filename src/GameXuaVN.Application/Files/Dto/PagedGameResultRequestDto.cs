using Abp.Application.Services.Dto;
using System;

namespace GameXuaVN.Games.Dto
{
    //custom PagedResultRequestDto
    public class PagedGameResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

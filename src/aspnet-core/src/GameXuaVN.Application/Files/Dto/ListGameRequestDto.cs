using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace GameXuaVN.Games.Dto
{
    //custom PagedResultRequestDto
    public class ListGameRequestDto : PagedResultRequestDto
    {
     
        public string Keyword { get; set; } = string.Empty;
        public string Page { get; set; } = string.Empty;
        public int CategoryId { get; set; } = -1;
    }


}

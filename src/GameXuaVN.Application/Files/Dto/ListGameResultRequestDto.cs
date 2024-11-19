using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace GameXuaVN.Games.Dto
{
    //custom PagedResultRequestDto
    public class ListGameResultRequestDto  
    {
        public List<GameDto> Data { get; set; }
    }
}

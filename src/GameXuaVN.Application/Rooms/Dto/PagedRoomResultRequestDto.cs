using Abp.Application.Services.Dto;
using System;

namespace GameXuaVN.Rooms.Dto
{
    //custom PagedResultRequestDto
    public class PagedRoomResultRequestDto : PagedResultRequestDto
    {
        

        public int CategoryId { get; set; } = -1;

        public string Keyword { get; set; }
    }
}

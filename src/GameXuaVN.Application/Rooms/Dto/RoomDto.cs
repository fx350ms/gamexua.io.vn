using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using GameXuaVN.Entities;

namespace GameXuaVN.Rooms.Dto
{
    [AutoMapFrom(typeof(Room))]
    public class RoomDto : EntityDto<long>
    {
        public string RoomName { get; set; }
        public int CategoryId { get; set; }
        public string HostPlayer { get; set; }
        public int MaxPlayers { get; set; }
        public int CurrentPlayers { get; set; }
        public bool IsOpen { get; set; }
    }
}

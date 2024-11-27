using Abp.AutoMapper;
using GameXuaVN.Entities;

namespace GameXuaVN.Rooms.Dto
{
    [AutoMapFrom(typeof(Room))]
    public class JoinRoomDto  
    {
        public long RoomId { get; set; }
        public string UserName {  get; set; }
    }
}

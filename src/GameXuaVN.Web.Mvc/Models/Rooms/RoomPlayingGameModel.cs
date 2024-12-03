using GameXuaVN.Games.Dto;
using GameXuaVN.Rooms.Dto;
using System.Collections.Generic;

namespace GameXuaVN.Web.Models.Rooms
{
    public class RoomPlayingGameModel
    {
        public RoomDto Room { get; set; }
        public GameDto Game { get; set; }
        public List<string> Players { get; set; }
    }
}

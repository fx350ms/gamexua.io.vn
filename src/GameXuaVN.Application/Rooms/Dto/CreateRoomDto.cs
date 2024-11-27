﻿using Abp.AutoMapper;
using GameXuaVN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameXuaVN.Rooms.Dto
{
    [AutoMapFrom(typeof(Room))]
    public class CreateRoomDto 
    {
        public string RoomName { get; set; }
        public string HostPlayer { get; set; }
        public int MaxPlayers { get; set; }
        public int CurrentPlayers { get; set; }
        public bool IsOpen { get; set; }
    }
}

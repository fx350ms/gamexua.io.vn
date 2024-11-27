using Abp.Application.Services;
using GameXuaVN.Games;
using GameXuaVN.Games.Dto;
using GameXuaVN.Roles.Dto;
using GameXuaVN.Rooms.Dto;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GameXuaVN.Rooms
{

    public interface IRoomAppService : IAsyncCrudAppService<RoomDto, long, PagedRoomResultRequestDto, CreateRoomDto, RoomDto>
    {
        Task JoinRoom(JoinRoomDto input);
        Task LeaveRoom(int roomId, long userId);
    }

}

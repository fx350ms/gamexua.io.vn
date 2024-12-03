using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using GameXuaVN.Entities;
using GameXuaVN.Rooms.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameXuaVN.Rooms
{
    public class RoomAppService : AsyncCrudAppService<Room, RoomDto, long, PagedRoomResultRequestDto, CreateRoomDto, RoomDto>, IRoomAppService
    {
        private readonly IRepository<Room, long> _roomRepository;
        private readonly IRepository<RoomParticipant, long> _participantRepository;

        public RoomAppService(IRepository<Room, long> roomRepository, IRepository<RoomParticipant, long> participantRepository)
            : base(roomRepository)
        {
            _roomRepository = roomRepository;
            _participantRepository = participantRepository;
        }

        //    public async Task<CreateRoomDto> CreateRoom(CreateRoomDto input);
        //    {
        //        var room = new Room
        //        {
        //            RoomName = input.RoomName,
        //            HostPlayer = input.HostPlayer,
        //            MaxPlayers = input.MaxPlayers,
        //            CurrentPlayers = 1,
        //            IsOpen = true
        //        };
        //        await _roomRepository.InsertAsync(room);
        //    }

        public override Task<RoomDto> CreateAsync(CreateRoomDto input)
        {
            try
            {
                input.CurrentPlayers = 1;
                input.IsOpen = true;
                input.HostPlayerId = AbpSession.GetUserId();

                return base.CreateAsync(input);
            }
            catch (System.Exception ex)
            {

                return null;
            }
           
        }

        public async Task<List<RoomDto>> GetAllRooms()
        {
            return ObjectMapper.Map<List<RoomDto>>(await _roomRepository.GetAllListAsync(r => r.IsOpen));
        }

        public async Task JoinRoom(JoinRoomDto input)
        {
            var room = await _roomRepository.GetAsync(input.RoomId);
            if (room.CurrentPlayers < room.MaxPlayers && room.IsOpen)
            {
                await _participantRepository.InsertAsync(new RoomParticipant
                {
                    RoomId = input.RoomId,
                    UserName = input.UserName,
                    IsReady = false
                });
                room.CurrentPlayers++;
                if (room.CurrentPlayers == room.MaxPlayers)
                {
                    room.IsOpen = false;
                }
                await _roomRepository.UpdateAsync(room);
            }
        }

        public async Task LeaveRoom(int roomId, long userId)
        {
            var participant = await _participantRepository.FirstOrDefaultAsync(p => p.RoomId == roomId && p.UserId == userId);
            if (participant != null)
            {
                await _participantRepository.DeleteAsync(participant);
                var room = await _roomRepository.GetAsync(roomId);
                room.CurrentPlayers--;
                room.IsOpen = true;
                await _roomRepository.UpdateAsync(room);
            }
        }

    }

}

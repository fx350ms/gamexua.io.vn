using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using GameXuaVN.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System;

namespace GameXuaVN.Web.Hubs
{
    [AbpAuthorize]
    public class GameHub : Hub
    {
        private readonly IRepository<Room, long> _roomRepository;
        private readonly IRepository<RoomParticipant, long> _participantRepository;
        private readonly IAbpSession _abpSession;
        public static int NumberOfUsers = 0;


        public GameHub(
            IRepository<Room, long> roomRepository,
            IRepository<RoomParticipant, long> participantRepository,
            IAbpSession abpSession)
        {
            _roomRepository = roomRepository;
            _participantRepository = participantRepository;
            _abpSession = abpSession;
        }
        public override Task OnConnectedAsync()
        {
            NumberOfUsers++;
            Clients.All.SendAsync("UserCountUpdated", NumberOfUsers);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            NumberOfUsers--;
            Clients.All.SendAsync("UserCountUpdated", NumberOfUsers);
            return base.OnDisconnectedAsync(exception);
        }


        //// Tạo room
        //public async Task CreateRoom(string roomName, int maxPlayers)
        //{
        //    var userId = _abpSession.UserId;
        //    var userName = Context.User.Identity.Name;

        //    if (!userId.HasValue)
        //    {
        //        throw new HubException("You must be logged in to create a room.");
        //    }

        //    // Tạo room
        //    var room = new Room
        //    {
        //        RoomName = roomName,
        //        HostPlayer = userName,
        //        MaxPlayers = maxPlayers,
        //        CurrentPlayers = 1,
        //        IsOpen = true
        //    };

        //    await _roomRepository.InsertAsync(room);

        //    // Thêm người tạo room vào danh sách participants
        //    await _participantRepository.InsertAsync(new RoomParticipant
        //    {
        //        RoomId = room.Id,
        //        UserId = userId.Value,
        //        UserName = userName,
        //        IsReady = false
        //    });

        //    // Thông báo cho client
        //    await Clients.Caller.roomCreated(room.Id, roomName);
        //}

        //// Tham gia room
        //public async Task JoinRoom(int roomId)
        //{
        //    var userId = _abpSession.UserId;
        //    var userName = Context.User.Identity.Name;

        //    if (!userId.HasValue)
        //    {
        //        throw new HubException("You must be logged in to join a room.");
        //    }

        //    var room = await _roomRepository.GetAsync(roomId);

        //    if (room.CurrentPlayers >= room.MaxPlayers || !room.IsOpen)
        //    {
        //        throw new HubException("Room is full or closed.");
        //    }

        //    // Thêm người chơi vào danh sách participants
        //    await _participantRepository.InsertAsync(new RoomParticipant
        //    {
        //        RoomId = roomId,
        //        UserId = userId.Value,
        //        UserName = userName,
        //        IsReady = false
        //    });

        //    room.CurrentPlayers++;
        //    if (room.CurrentPlayers == room.MaxPlayers)
        //    {
        //        room.IsOpen = false;
        //    }

        //    await _roomRepository.UpdateAsync(room);

        //    // Thông báo cho tất cả client trong room
        //    await Groups.Add(Context.ConnectionId, roomId.ToString());
        //    await Clients.Group(roomId.ToString()).playerJoined(userName);
        //}

        //// Rời room
        //public async Task LeaveRoom(int roomId)
        //{
        //    var userId = _abpSession.UserId;

        //    if (!userId.HasValue)
        //    {
        //        throw new HubException("You must be logged in to leave a room.");
        //    }

        //    var participant = await _participantRepository.FirstOrDefaultAsync(p => p.RoomId == roomId && p.UserId == userId);
        //    if (participant == null)
        //    {
        //        throw new HubException("You are not part of this room.");
        //    }

        //    await _participantRepository.DeleteAsync(participant);

        //    var room = await _roomRepository.GetAsync(roomId);
        //    room.CurrentPlayers--;

        //    if (room.CurrentPlayers == 0)
        //    {
        //        // Đóng room nếu không còn ai
        //        room.IsOpen = false;
        //    }

        //    await _roomRepository.UpdateAsync(room);

        //    // Thông báo cho client
        //    await Groups.Remove(Context.ConnectionId, roomId.ToString());
        //    await Clients.Group(roomId.ToString()).playerLeft(Context.User.Identity.Name);
        //}

        //// Đồng bộ trạng thái game
        //public async Task SyncGameState(int roomId, string gameState)
        //{
        //    await Clients.Group(roomId.ToString()).receiveGameState(gameState);
        //}
    }
}

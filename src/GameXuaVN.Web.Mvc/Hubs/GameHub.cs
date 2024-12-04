﻿using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using GameXuaVN.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System;
using GameXuaVN.Web.Models.Rooms;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace GameXuaVN.Web.Hubs
{
    [AbpAuthorize]
    public class GameHub : Hub
    {
        private readonly IRepository<Entities.Room, long> _roomRepository;
        private readonly IRepository<RoomParticipant, long> _participantRepository;
        private readonly IAbpSession _abpSession;
        public static int NumberOfUsers = 0;


        public GameHub(
            IRepository<Entities.Room, long> roomRepository,
            IRepository<RoomParticipant, long> participantRepository,
            IAbpSession abpSession)
        {
            _roomRepository = roomRepository;
            _participantRepository = participantRepository;
            _abpSession = abpSession;
        }



        private static ConcurrentDictionary<string, Room> Rooms = new ConcurrentDictionary<string, Room>();

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


        private void Disconnect(string connectionId)
        {
            var room = Rooms.Values.FirstOrDefault(r => r.Users.Any(u => u.ConnectionId == connectionId));
            if (room == null) return;

            var user = room.Users.FirstOrDefault(u => u.ConnectionId == connectionId);
            if (user != null)
            {
                room.Users.Remove(user);
            }

            // Gửi thông báo người dùng đã rời phòng
            Clients.Group(room.Id).SendAsync("user-disconnected", user.UserId);

            if (!room.Users.Any())
            {
                // Nếu phòng trống, xóa phòng
                Rooms.TryRemove(room.Id, out _);
            }
            else if (room.Owner.UserId == user.UserId)
            {
                // Nếu người rời là chủ phòng, chuyển quyền chủ phòng
                room.Owner = room.Users.First();
                Clients.Client(room.Owner.ConnectionId).SendAsync("set-isInitiator-true", room.SessionId);
            }
        }

        public async Task OpenRoom(RoomData data)
        {
            var room = new Room
            {
                Domain = data.Extra.Domain,
                GameId = data.Extra.GameId,
                SessionId = data.SessionId,
                RoomName = data.Extra.RoomName,
                MaxParticipantsAllowed = data.MaxParticipantsAllowed,
                CurrentParticipants = 1,
                Password = data.Password.Trim(),
                CoreVersion = data.CoreVer,
                Users = new List<User>
            {
                new User
                {
                    UserId = data.UserId,
                    ConnectionId = Context.ConnectionId,
                    ExtraData = data.Extra
                }
            },
                Owner = new User
                {
                    UserId = data.UserId,
                    ConnectionId = Context.ConnectionId,
                    ExtraData = data.Extra
                }
            };

            Rooms.TryAdd(room.Id, room);

            // Thêm vào nhóm SignalR đại diện cho phòng
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);

            // Gửi thông báo tới client
            await Clients.Caller.SendAsync("extra-data-updated", null, room.Users.First().ExtraData);
            await Clients.Caller.SendAsync("extra-data-updated", data.UserId, room.Users.First().ExtraData);

            // Xác nhận mở phòng thành công
            await Clients.Caller.SendAsync("open-room-callback", true, null);
        }

        public async Task CheckPresence(string roomId)
        {
            var roomExists = Rooms.ContainsKey(roomId);
            await Clients.Caller.SendAsync("check-presence-callback", roomExists, roomId, null);
        }

        public async Task JoinRoom(RoomJoinData data)
        {
            if (!Rooms.TryGetValue(data.SessionId, out var room))
            {
                await Clients.Caller.SendAsync("join-room-callback", false, "USERID_NOT_AVAILABLE");
                return;
            }

            if (room.CurrentParticipants >= room.MaxParticipantsAllowed)
            {
                await Clients.Caller.SendAsync("join-room-callback", false, "ROOM_FULL");
                return;
            }

            if (!string.IsNullOrEmpty(room.Password) && room.Password != data.Password)
            {
                await Clients.Caller.SendAsync("join-room-callback", false, "INVALID_PASSWORD");
                return;
            }

            var newUser = new User
            {
                UserId = data.UserId,
                ConnectionId = Context.ConnectionId,
                ExtraData = data.Extra
            };

            room.Users.Add(newUser);
            room.CurrentParticipants++;

            // Gửi thông báo tới các thành viên khác trong phòng
            await Clients.Group(room.Id).SendAsync("user-connected", data.UserId);

            // Thêm vào nhóm SignalR
            await Groups.AddToGroupAsync(Context.ConnectionId, room.Id);

            // Xác nhận tham gia phòng thành công
            await Clients.Caller.SendAsync("join-room-callback", true, null);
        }

        public async Task SetPassword(string roomId, string password)
        {
            if (!Rooms.TryGetValue(roomId, out var room)) return;

            room.Password = password?.Trim();
            await Clients.Caller.SendAsync("set-password-callback", true);
        }

        public async Task ExtraDataUpdated(string roomId, object data)
        {
            if (!Rooms.TryGetValue(roomId, out var room)) return;

            room.ExtraData = data;

            // Gửi thông báo cập nhật dữ liệu tới tất cả các thành viên
            await Clients.Group(room.Id).SendAsync("extra-data-updated", room.Owner.UserId, data);
        }

        public async Task GetRemoteUserExtraData(string roomId, string userId)
        {
            if (!Rooms.TryGetValue(roomId, out var room)) return;

            var user = room.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                await Clients.Caller.SendAsync("extra-data-updated", user.ExtraData);
            }
        }


        public Task CreateRoom(long roomId, int gameId, string roomName, string gameName)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            Clients.All.SendAsync("OnRoomCreated", roomId, gameId, roomName, gameName);
            return Task.CompletedTask;
        }



        public async Task Netplay(string roomId, NetplayMessage msg)
        {
            if (!Rooms.TryGetValue(roomId, out var room))
            {
                return; // Nếu không tìm thấy phòng, thoát
            }

            // Tạo bản sao tin nhắn và gắn thêm dữ liệu "extraData"
            var outMsg = new NetplayMessage
            {
                Message = msg.Message,
                Sender = msg.Sender,
                Extra = msg.Extra // Lấy extraData từ phòng
            };

            // Gửi tin nhắn tới tất cả client trong phòng, ngoại trừ sender
            await Clients.Group(roomId).SendAsync("netplay", outMsg);

            // Kiểm tra nếu userLeft === true, thì xử lý ngắt kết nối
            if (msg.Message?.UserLeft == true)
            {
                await DisconnectFromRoom(roomId, msg.Sender);
            }
        }


        private async Task DisconnectFromRoom(string roomId, string userId)
        {
            // Kiểm tra nếu phòng tồn tại
            if (!Rooms.TryGetValue(roomId, out var room))
            {
                return; // Không tìm thấy phòng
            }

            // Tìm người dùng trong danh sách phòng
            var user = room.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return; // Không tìm thấy người dùng
            }

            // Loại bỏ người dùng khỏi phòng
            room.Users.Remove(user);

            // Gửi thông báo người dùng đã rời phòng tới tất cả thành viên trong phòng
            await Clients.Group(roomId).SendAsync("user-disconnected", userId);

            // Kiểm tra nếu phòng trống
            if (!room.Users.Any())
            {
                // Nếu phòng trống, xóa phòng khỏi danh sách
                Rooms.TryRemove(roomId, out _);
            }
            else if (room.Owner.UserId == userId)
            {
                // Nếu người rời là chủ phòng, chuyển quyền chủ phòng
                room.Owner = room.Users.First();
                await Clients.Client(room.Owner.ConnectionId).SendAsync("set-isInitiator-true", room.SessionId);
            }

            // Xóa người dùng khỏi nhóm SignalR
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
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

    // Các lớp hỗ trợ
    public class Room
    {
        public string Domain { get; set; }
        public int GameId { get; set; }
        public string SessionId { get; set; }
        public string RoomName { get; set; }
        public int MaxParticipantsAllowed { get; set; }
        public int CurrentParticipants { get; set; }
        public string Password { get; set; }
        public int CoreVersion { get; set; }
        public List<User> Users { get; set; } = new List<User>();
        public User Owner { get; set; }
        public object ExtraData { get; set; }

        public string Id => $"{Domain}:{GameId}:{SessionId}";
    }

    public class User
    {
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public object ExtraData { get; set; }
    }

    public class RoomData
    {
        public RoomExtra Extra { get; set; }
        public string SessionId { get; set; }
        public int MaxParticipantsAllowed { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
        public int CoreVer { get; set; }
    }

    public class RoomExtra
    {
        public string Domain { get; set; }
        public int GameId { get; set; }
        public string RoomName { get; set; }
    }

    public class RoomJoinData
    {
        public RoomExtra Extra { get; set; }
        public string SessionId { get; set; }
        public string Password { get; set; }
        public string UserId { get; set; }
    }

    public class Message
    {
        /// <summary>
        /// ID của người gửi tin nhắn
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// Nội dung tin nhắn hoặc thông điệp cần gửi
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Thông tin bổ sung (extra data) đi kèm tin nhắn
        /// </summary>
        public object Extra { get; set; }

        /// <summary>
        /// Ràng buộc SDP từ phía người gửi
        /// </summary>
        public SdpConstraints LocalPeerSdpConstraints { get; set; }

        /// <summary>
        /// Ràng buộc SDP từ phía người nhận
        /// </summary>
        public SdpConstraints RemotePeerSdpConstraints { get; set; }

        /// <summary>
        /// Xác định xem đây có phải tin nhắn chỉ chứa dữ liệu hay không
        /// </summary>
        public bool IsDataOnly { get; set; }

        /// <summary>
        /// Đánh dấu đây là yêu cầu tham gia mới
        /// </summary>
        public bool NewParticipationRequest { get; set; }

        /// <summary>
        /// Đánh dấu người dùng đã rời khỏi phòng
        /// </summary>
        public bool UserLeft { get; set; }
    }
    public class SdpConstraints
    {
        /// <summary>
        /// Ràng buộc SDP cho âm thanh
        /// </summary>
        public bool OfferToReceiveAudio { get; set; }

        /// <summary>
        /// Ràng buộc SDP cho video
        /// </summary>
        public bool OfferToReceiveVideo { get; set; }
    }

    public class NetplayMessage
    {
        public Message Message { get; set; }
        public string Sender { get; set; }
        public object Extra { get; set; }

    }
}

using Abp.Domain.Entities;

namespace GameXuaVN.Entities
{
    public class RoomParticipant : Entity<long>
    {
        public long RoomId { get; set; }
        public long UserId { get; set; }  // Sử dụng UserId để liên kết với bảng người dùng
        public string UserName { get; set; }  // Lưu tên người dùng
        public bool IsReady { get; set; }
    }
}

using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameXuaVN.Entities
{
    public class Room : FullAuditedEntity<long>
    {
        public string RoomName { get; set; }
        public int CategoryId { get; set; }
        public string HostPlayer { get; set; }
        public int MaxPlayers { get; set; }
        public int CurrentPlayers { get; set; }
        public bool IsOpen { get; set; }
    }
}

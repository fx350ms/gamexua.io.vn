using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GameXuaVN.Entities
{
    public class Score : FullAuditedEntity<int>
    {
        public string PlayerName { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public long Value { get; set; }
        public int ScoreType { get; set; }
    }
}

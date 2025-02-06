using Abp.Application.Services.Dto;
using GameXuaVN.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace GameXuaVN.Scores.Dto
{
    public class ScoreDto : EntityDto<int>
    {
        public string PlayerName { get; set; }
        public int GameId { get; set; }
        public long Value { get; set; }
        public ScoreType Type { get; set; }
    }

}

using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameXuaVN.Scores.Dto
{
    public class CreateScoreDto : EntityDto<int>
    {
        [Required]
        public string PlayerName { get; set; }
        [Required]
        public int GameId { get; set; }
        [Required]
        public long Value { get; set; }
        [Required]
        public ScoreType Type { get; set; }
    }
}

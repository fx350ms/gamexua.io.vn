﻿using Abp.Application.Services.Dto;

namespace GameXuaVN.Scores.Dto
{
    public class ListScoreRequestDto : PagedResultRequestDto
    {
        public int GameId { get; set; }
        public ScoreType? Type { get; set; }
    }

}

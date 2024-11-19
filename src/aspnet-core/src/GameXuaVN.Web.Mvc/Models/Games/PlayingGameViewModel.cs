using GameXuaVN.Games.Dto;
using System.Collections.Generic;

namespace GameXuaVN.Web.Models.Games
{
    public class PlayingGameViewModel
    {
        public long Id { get; set; }
        public GameDto Data { get; set; }

        public List<GameDto> TopList { get; set; }
    }
}

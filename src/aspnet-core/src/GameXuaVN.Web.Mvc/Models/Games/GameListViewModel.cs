using GameXuaVN.Games.Dto;
using System.Collections.Generic;

namespace GameXuaVN.Web.Models.Games
{
    public class GameListViewModel
    {
        public IReadOnlyList<GameDto> Data {  get; set; }
    }
}

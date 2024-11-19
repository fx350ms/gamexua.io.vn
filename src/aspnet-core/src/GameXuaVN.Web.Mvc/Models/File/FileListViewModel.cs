using System.Collections.Generic;
using GameXuaVN.Games.Dto;
using GameXuaVN.Roles.Dto;

namespace GameXuaVN.Web.Models.File
{
    public class FileListViewModel
    {
        public IReadOnlyList<GameDto> Data { get; set; }
    }
}

using GameXuaVN.Controllers;
using GameXuaVN.Games;
using GameXuaVN.Rooms;
using GameXuaVN.Web.Models.Games;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameXuaVN.Web.Controllers
{
    public class RoomController : GameXuaVNControllerBase
    {
        private readonly IRoomAppService _roomAppService;
        private readonly IRoomAppService _roomParticalAppService;

        public RoomController(IRoomAppService roomAppService)
        {
            _roomAppService = roomAppService;
        }

        public async Task<IActionResult> Index()
        {
            //var roomsList = await _roomAppService.GetAllAsync(new Rooms.Dto.PagedRoomResultRequestDto() { });
            //return View(roomsList.Items);

            return View();
        }

    }
}

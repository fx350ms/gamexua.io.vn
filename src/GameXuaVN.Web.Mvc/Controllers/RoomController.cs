using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using GameXuaVN.Controllers;
using GameXuaVN.Games;
using GameXuaVN.Rooms;
using GameXuaVN.Web.Models.Games;
using GameXuaVN.Web.Models.Rooms;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace GameXuaVN.Web.Controllers
{

    [AbpMvcAuthorize]
    public class RoomController : GameXuaVNControllerBase
    {
        private readonly IRoomAppService _roomAppService;
        
        private readonly IGameAppService _gameAppService;
        public RoomController(IRoomAppService roomAppService, IGameAppService gameAppService)
        {
            _roomAppService = roomAppService;
            _gameAppService = gameAppService;
        }

        public async Task<IActionResult> Index()
        {
            var roomsList = await _roomAppService.GetAllAsync(new Rooms.Dto.PagedRoomResultRequestDto() { });
            return View(roomsList.Items);
        }


        public async Task<IActionResult> Join(long id)
        {
            var dto = await _roomAppService.GetAsync(new EntityDto<long>(id));
            var gameDto = await _gameAppService.GetAsync(new EntityDto<int>(dto.GameId));

            await _gameAppService.UpdateAsync(gameDto);

            var model = new RoomPlayingGameModel()
            {
                Room = dto,
                Game = gameDto,
                Players = new List<string>() { User.Identity.Name }
            };
            return View(model);
        }

    }
}

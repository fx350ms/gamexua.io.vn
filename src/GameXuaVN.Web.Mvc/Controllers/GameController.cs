using Abp.Application.Services.Dto;
using GameXuaVN.Controllers;
using GameXuaVN.Games;
using GameXuaVN.Web.Models.File;
using GameXuaVN.Web.Models.Games;
using GameXuaVN.Web.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GameXuaVN.Web.Controllers
{
    public class GameController : GameXuaVNControllerBase
    {
        private readonly IGameAppService _fileAppService;

        public GameController(IGameAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }

         

        public async Task<IActionResult> Index()
        {
            var model = new GameListViewModel
            {

            };
            return View(model);
        }


        public async Task<ActionResult> EditModal(int gameId)
        {
            var game = await _fileAppService.GetAsync(new EntityDto<int>(gameId));
           
            var model = new CreateOrUpdateGameModel
            {
               Name = game.Name,
               Data = game.Data,
               CategoryId = game.CategoryId,
               Description = game.Description,
               TotalDislike = game.TotalDislike,
               TotalLike = game.TotalLike,
               TotalPlay = game.TotalPlay,
              
            };
            return PartialView("_EditModal", model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> GetFile(int id)
        {
            var dto = await _fileAppService.GetAsync(new EntityDto<int>(id));
            return File(dto.Data, "application/octet-stream");
        }
    }
}

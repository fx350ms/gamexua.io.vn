using GameXuaVN.Controllers;
using GameXuaVN.Games;
using GameXuaVN.Games.Dto;
using GameXuaVN.Web.Models.Games;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace GameXuaVN.Web.Controllers
{
    public class NesController : GameXuaVNControllerBase
    {

        private readonly IGameAppService _gameAppService;

        public NesController(IGameAppService gameAppService)
        {
            _gameAppService = gameAppService;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }


        public async Task<IActionResult> TopPlay()
        {
            return View();
        }


        public async Task<IActionResult> TopLike()
        {
            return View();
        }

        public async Task<IActionResult> TopPlayListItem(string page = "")
        {
            var dto = await _gameAppService.GetTopPlayList(new ListGameRequestDto()
            {
                CategoryId = -1,
                Page = page,
                MaxResultCount = 24
            });
            var model = new GameListViewModel()
            {
                Data = dto.Data,
            };
            return View(model);
        }

        public async Task<IActionResult> ListItem(string page = "#" )
        {
            var dto = await _gameAppService.GetListAsync(new ListGameRequestDto()
            {
                CategoryId = -1,
                Page = page,
            });
            var model = new GameListViewModel()
            {
                Data = dto.Data,
            };
            return View(model);
        }

        public async Task<IActionResult> Play(int id, string title)
        {
            var dto = await _gameAppService.GetAsync(new EntityDto<int>(id));

            dto.TotalPlay++;
            await _gameAppService.UpdateAsync(dto);
            var model = new PlayingGameViewModel()
            {
                Id = id,
                Data = dto
            };

            return View(model);
        }


        public async Task<IActionResult> Detail(int id, string title)
        {
            var dto = await _gameAppService.GetAsync(new EntityDto<int>(id));
            var topList = await _gameAppService.GetTopPlayList(new ListGameRequestDto()
            {
                MaxResultCount = 8
            });
            var model = new PlayingGameViewModel()
            {
                Id = id,
                Data = dto,
                TopList = topList.Data
            };
            return View(model);
        }


        public async Task<IActionResult> GetFile(int id)
        {
            var dto = await _gameAppService.GetAsync(new EntityDto<int>(id));
            return File(dto.Data, "application/octet-stream");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadRom(int id)
        {
            var game = await _gameAppService.GetAsync(new EntityDto<int>(id));

            if (game == null || game.Data == null)
            {
                return NotFound();
            }

            // Trả về file ROM với MIME Type và tên file
            return File(game.Data, "application/octet-stream", $"{game.Name}.nes");
        }
    }
}

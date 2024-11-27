using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using GameXuaVN.Controllers;
using GameXuaVN.Games;
using GameXuaVN.Games.Dto;
using GameXuaVN.Web.Models.File;
using GameXuaVN.Web.Models.Games;
using GameXuaVN.Web.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;

namespace GameXuaVN.Web.Controllers
{

    [AbpMvcAuthorize]
    public class GameController : GameXuaVNControllerBase
    {
        private readonly IGameAppService _gameAppService;

        public GameController(IGameAppService gameAppService)
        {
            _gameAppService = gameAppService;
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
            var game = await _gameAppService.GetAsync(new EntityDto<int>(gameId));

            //var model = new GameDto
            //{
            //    Id = game.Id,
            //    Name = game.Name,
            //    Data = game.Data,
            //    CategoryId = game.CategoryId,
            //    Description = game.Description,
            //    TotalDislike = game.TotalDislike,
            //    TotalLike = game.TotalLike,
            //    TotalPlay = game.TotalPlay,

            //};
            return PartialView("_EditModal", game);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateGameDto input)
        {
            if (input.ThumbnailFromFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await input.ThumbnailFromFile.CopyToAsync(memoryStream);
                    input.Thumbnail = memoryStream.ToArray();
                }
            }

            // Xử lý Data
            if (input.DataFromFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await input.DataFromFile.CopyToAsync(memoryStream);
                    input.Data = memoryStream.ToArray();
                    input.ContentType = input.DataFromFile.ContentType; // Lưu MIME Type
                }
            }


            // Gọi service để lưu dữ liệu
            var result = await _gameAppService.CreateAsync(input);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] GameDto input)
        {
            if (input.ThumbnailFromFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await input.ThumbnailFromFile.CopyToAsync(memoryStream);
                    input.Thumbnail = memoryStream.ToArray();
                }
            }

            // Xử lý Data
            if (input.DataFromFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await input.DataFromFile.CopyToAsync(memoryStream);
                    input.Data = memoryStream.ToArray();
                    input.ContentType = input.DataFromFile.ContentType; // Lưu MIME Type
                }
            }


            // Gọi service để lưu dữ liệu
            var result = await _gameAppService.UpdateAsync(input);

            return Ok(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> GetFile(int id)
        {
            var dto = await _gameAppService.GetAsync(new EntityDto<int>(id));
            return File(dto.Data, "application/octet-stream");
        }
    }
}

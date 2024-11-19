using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using GameXuaVN.Controllers;
using GameXuaVN.Games;
using GameXuaVN.Games.Dto;
using GameXuaVN.Web.Models.File;
using GameXuaVN.Web.Models.Games;
using GameXuaVN.Web.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;


namespace GameXuaVN.Web.Controllers
{
    [AbpMvcAuthorize]
    public class GameController : GameXuaVNControllerBase
    {
        private readonly IGameAppService _gameAppService;

        public GameController(IGameAppService fileAppService)
        {
            _gameAppService = fileAppService;
        }


        public async Task<IActionResult> Index()
        {
            var model = new GameListViewModel
            {
            };
            return View(model);
        }


        [HttpPost]
        public async Task<JsonResult> GetAll([FromBody] ListGameRequestDto input)
        {
            // Gọi service để lấy danh sách game theo phân trang
            var games = await _gameAppService.GetListAsync(new ListGameRequestDto() { }) ;
            //_gameAppService.GetAllAsync();
            //return Json(new
            //{
            //    draw = Request.Form["draw"].FirstOrDefault(), // DataTables cần giá trị này
            //    recordsTotal = games.TotalCount,            // Tổng số bản ghi (trước khi phân trang)
            //    recordsFiltered = games.TotalCount,        // Tổng số bản ghi sau khi áp dụng filter
            //    data = games.Items                         // Dữ liệu cần hiển thị
            //});

            return Json(new object());
        }


        public async Task<ActionResult> EditModal(int gameId)
        {
            var game = await _gameAppService.GetAsync(new EntityDto<int>(gameId));
           
            //var model = new CreateOrUpdateGameModel
            //{
            //   Name = game.Name,
            //   Data = game.Data,
            //   CategoryId = game.CategoryId,
            //   Description = game.Description,
            //   TotalDislike = game.TotalDislike,
            //   TotalLike = game.TotalLike,
            //   TotalPlay = game.TotalPlay,
              
            //};
            return PartialView("_EditModal", game);
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

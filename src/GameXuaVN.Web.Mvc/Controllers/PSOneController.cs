using GameXuaVN.Controllers;
using GameXuaVN.Games;
using GameXuaVN.Games.Dto;
using GameXuaVN.Web.Models.Games;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace GameXuaVN.Web.Controllers
{
    public class PSOneController : GameXuaVNControllerBase
    {

        private readonly IGameAppService _fileAppService;

        public PSOneController(IGameAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }


        public async Task<IActionResult> Index()
        {
            return RedirectToAction("CommingSoon", "Home");
        }

    }
}

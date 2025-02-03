using GameXuaVN.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GameXuaVN.Web.Controllers
{
    public class BrainController : GameXuaVNControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Basic()
        {
            return View();
        }
        public IActionResult Arcade()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using GameXuaVN.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GameXuaVN.Web.Controllers
{
   
    public class HomeController : GameXuaVNControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }


        public ActionResult Index()
        {
            ViewBag.Environment = _environment.EnvironmentName;
            return View();
        }


        public ActionResult TopGame()
        {
            return View();
        }



        public ActionResult AboutMe()
        {
            return View();
        }

        public ActionResult CommingSoon()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get()
        {
            // Ghi log thông tin về môi trường
            _logger.LogInformation("HomeController is running in environment: {Environment}", _environment.EnvironmentName);
            return Ok(new { Environment = _environment.EnvironmentName });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using GameXuaVN.Controllers;

namespace GameXuaVN.Web.Controllers
{
    [AbpMvcAuthorize]
    public class DashboardController : GameXuaVNControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

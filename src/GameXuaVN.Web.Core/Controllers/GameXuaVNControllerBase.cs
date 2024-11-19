using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace GameXuaVN.Controllers
{
    public abstract class GameXuaVNControllerBase: AbpController
    {
        protected GameXuaVNControllerBase()
        {
            LocalizationSourceName = GameXuaVNConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}

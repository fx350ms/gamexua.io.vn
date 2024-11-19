using Abp.AspNetCore.Mvc.ViewComponents;

namespace GameXuaVN.Web.Views
{
    public abstract class GameXuaVNViewComponent : AbpViewComponent
    {
        protected GameXuaVNViewComponent()
        {
            LocalizationSourceName = GameXuaVNConsts.LocalizationSourceName;
        }
    }
}

using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using GameXuaVN.Configuration.Dto;

namespace GameXuaVN.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : GameXuaVNAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}

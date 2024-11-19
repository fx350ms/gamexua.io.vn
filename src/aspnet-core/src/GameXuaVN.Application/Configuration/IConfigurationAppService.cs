using System.Threading.Tasks;
using GameXuaVN.Configuration.Dto;

namespace GameXuaVN.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}

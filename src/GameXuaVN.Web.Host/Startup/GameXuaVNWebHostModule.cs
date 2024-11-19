using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using GameXuaVN.Configuration;

namespace GameXuaVN.Web.Host.Startup
{
    [DependsOn(
       typeof(GameXuaVNWebCoreModule))]
    public class GameXuaVNWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public GameXuaVNWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GameXuaVNWebHostModule).GetAssembly());
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using GameXuaVN.Configuration;

namespace GameXuaVN.Web.Startup
{
    [DependsOn(typeof(GameXuaVNWebCoreModule))]
    public class GameXuaVNWebMvcModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public GameXuaVNWebMvcModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.MultiTenancy.IsEnabled = false;
            Configuration.Navigation.Providers.Add<GameXuaVNNavigationProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GameXuaVNWebMvcModule).GetAssembly());
        }
    }
}

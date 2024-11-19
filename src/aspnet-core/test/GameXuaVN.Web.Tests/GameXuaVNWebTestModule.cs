using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using GameXuaVN.EntityFrameworkCore;
using GameXuaVN.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace GameXuaVN.Web.Tests
{
    [DependsOn(
        typeof(GameXuaVNWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class GameXuaVNWebTestModule : AbpModule
    {
        public GameXuaVNWebTestModule(GameXuaVNEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GameXuaVNWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(GameXuaVNWebMvcModule).Assembly);
        }
    }
}
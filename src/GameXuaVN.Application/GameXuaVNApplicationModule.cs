using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using GameXuaVN.Authorization;
using GameXuaVN.Games.Dto;
using GameXuaVN.Entities;
using GameXuaVN.Rooms.Dto;
namespace GameXuaVN
{
    [DependsOn(
        typeof(GameXuaVNCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class GameXuaVNApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<GameXuaVNAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                cfg.CreateMap<CreateGameDto, Game>();
                cfg.CreateMap<GameDto, Game>();
                cfg.CreateMap<Game, GameDto>();

                cfg.CreateMap<RoomDto, Room>().ReverseMap();
                cfg.CreateMap<CreateRoomDto, Room>().ReverseMap();

            });
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(GameXuaVNApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}

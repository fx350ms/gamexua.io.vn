using System.Threading.Tasks;
using Abp.Application.Services;
using GameXuaVN.Sessions.Dto;

namespace GameXuaVN.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}

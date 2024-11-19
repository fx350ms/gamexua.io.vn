using System.Threading.Tasks;
using Abp.Application.Services;
using GameXuaVN.Authorization.Accounts.Dto;

namespace GameXuaVN.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}

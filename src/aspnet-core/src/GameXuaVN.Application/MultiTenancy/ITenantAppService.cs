using Abp.Application.Services;
using GameXuaVN.MultiTenancy.Dto;

namespace GameXuaVN.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}


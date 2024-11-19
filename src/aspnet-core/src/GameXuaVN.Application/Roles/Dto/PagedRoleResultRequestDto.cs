using Abp.Application.Services.Dto;

namespace GameXuaVN.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}


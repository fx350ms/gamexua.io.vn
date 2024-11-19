using System.Collections.Generic;
using GameXuaVN.Roles.Dto;

namespace GameXuaVN.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}

using System.Collections.Generic;
using GameXuaVN.Roles.Dto;

namespace GameXuaVN.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}

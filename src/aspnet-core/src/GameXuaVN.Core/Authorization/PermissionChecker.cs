using Abp.Authorization;
using GameXuaVN.Authorization.Roles;
using GameXuaVN.Authorization.Users;

namespace GameXuaVN.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}

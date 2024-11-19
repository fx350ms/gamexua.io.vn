using System.Collections.Generic;
using GameXuaVN.Roles.Dto;

namespace GameXuaVN.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}
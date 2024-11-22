using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using GameXuaVN.Authorization;

namespace GameXuaVN.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class GameXuaVNNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                //.AddItem(
                //    new MenuItemDefinition(
                //        PageNames.About,
                //        L("About"),
                //        url: "About",
                //        icon: "fas fa-info-circle"
                //    )
                //)
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Home,
                        L("HomePage"),
                        url: "",
                        icon: "fas fa-home",
                        requiresAuthentication: true
                    )
                )
                //.AddItem(
                //    new MenuItemDefinition(
                //        PageNames.Tenants,
                //        L("Tenants"),
                //        url: "Tenants",
                //        icon: "fas fa-building",
                //        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
                //    )
                //)

                .AddItem(
                    new MenuItemDefinition(
                        PageNames.Users,
                        L("Users"),
                        url: "Users",
                        icon: "fas fa-users",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Roles,
                        L("Roles"),
                        url: "Roles",
                        icon: "fas fa-theater-masks",
                        permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
                    )
                )
                .AddItem(
                     new MenuItemDefinition(
                         PageNames.Tenants,
                         L("GameList"),
                         url: "Game",
                         icon: "fas fa-gamepad"
                         
                     )
                 )
                 .AddItem(
                     new MenuItemDefinition(
                         PageNames.Tenants,
                         L("Category"),
                         url: "category",
                         icon: "fas fa-folder"

                     )
                 )
                ;
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, GameXuaVNConsts.LocalizationSourceName);
        }
    }
}
using System.Threading.Tasks;
using GameXuaVN.Models.TokenAuth;
using GameXuaVN.Web.Controllers;
using Shouldly;
using Xunit;

namespace GameXuaVN.Web.Tests.Controllers
{
    public class HomeController_Tests: GameXuaVNWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<DashboardController>(nameof(DashboardController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}
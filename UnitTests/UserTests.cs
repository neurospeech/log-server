using LogServer.Web.Areas.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTests
{
    public class UserTests : BaseTest
    {
        public UserTests(ITestOutputHelper writer) : base(writer)
        {
        }


        [Fact(DisplayName = "User - CreateApp")]
        public async Task CreateApp() {

            AppController appController = new AppController();

            var a = await appController.Put(new LogServer.Web.Models.App
            {
                Name = "Sample",
                Platform = "iOS"
            });

            LogController logController = new LogController();

            //Assert.AreNotEqual(appController.DIScope, logController.DIScope);

            await logController.Put(a.AppID, a.AppPushKey, new LogInfo
            {
                Application = "Sample",

                Title = "Title",
                Detail = "Description",
                Device = "Log",
                User = "a"
            });

            await logController.Put(a.AppID, a.AppPushKey, new LogInfo
            {
                Application = "Sample",

                Title = "Title",
                Detail = "Description",
                Device = "Log",
                User = "a"
            });

            await logController.Clean();

            Assert.Equal(a.AppID, 1);

        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogServer.Web.Areas.Api.Controllers;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class UnitTest: BaseTest
    {
        [TestMethod]
        public async Task CreateApp()
        {

            AppController appController = new AppController();

            var a = await appController.Put(new LogServer.Web.Models.App {
                Name = "Sample",
                Platform = "iOS"
            });

            LogController logController = new LogController();

            //Assert.AreNotEqual(appController.DIScope, logController.DIScope);

            await logController.Put(a.AppID, a.AppPushKey, new LogInfo {
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

            Assert.AreEqual(a.AppID, 1);

        }
    }
}

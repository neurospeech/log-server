using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LogServer.Web.Areas.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TestController: ApiController
    {

        /// <summary>
        /// Verifies installation
        /// </summary>
        /// <returns></returns>
        [Route("api/test/version")]
        [HttpGet]
        public string Get() {
            return "1.0";
        }

    }
}
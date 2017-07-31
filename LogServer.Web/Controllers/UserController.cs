using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LogServer.Web.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public class UserController : BaseController
    {


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Login(string redirectUrl = null) {
            if (string.IsNullOrWhiteSpace(redirectUrl)) {
                redirectUrl = "/";
            }
            return View((object) redirectUrl);
        } 

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogServer.Web.Areas.Api.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class LoginModel
    {

        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RememberMe { get; set; }

    }
}
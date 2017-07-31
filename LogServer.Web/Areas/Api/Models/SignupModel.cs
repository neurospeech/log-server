using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogServer.Web.Areas.Api.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class SignupModel
    {

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required, MinLength(5)]
        public string Username { get; set; }
    }
}
using LogServer.Web;
using LogServer.Web.Models;
using LogServer.Web.Services;
using NeuroSpeech.CoreDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

//[assembly: PreApplicationStartMethod(typeof(DIConfig), "Register")]

namespace LogServer.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class DIConfig
    {

        /// <summary>
        /// 
        /// </summary>
        public static void Register() {
            DI.Register(typeof(DIConfig).Assembly);
            AppDbContext.InitializeMigration();
        }

    }
}
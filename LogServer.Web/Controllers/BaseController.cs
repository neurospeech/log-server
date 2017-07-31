using LogServer.Web.Models;
using NeuroSpeech.CoreDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogServer.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseController: Controller
    {
        /// <summary>
        /// 
        /// </summary>
        protected DIScope DIScope { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        protected AppDbContext DB { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseController()
        {
            DIScope = DI.NewScope();

            DB = Get<AppDbContext>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T Get<T>()
        {
            return DI.Get<T>(DIScope);
        }
    }
}
using LogServer.Web.Models;
using LogServer.Web.Services;
using NeuroSpeech.CoreDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LogServer.Web.Areas.Api.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class ApiDbController : ApiController
    {

        /// <summary>
        /// 
        /// </summary>
        public ApiDbController()
        {
            //DB = new AppDbContext();

            DIScope = DI.NewScope();

            DB = Get<AppDbContext>();
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ApiKeyHeader { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public AppDbContext DB { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DIScope DIScope { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        protected bool IsUserAdmin
        {
            get
            {
                return DB.WebUsers.Any(x => x.Username == Username && x.IsAdmin == true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string Username
        {
             get
            {
                var authService = Get<IAuthService>();
                return authService.GetUsername(this.Request, this.User);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                DIScope?.Dispose();
                DIScope = null;
            }
            base.Dispose(disposing);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        protected void ThrowHttpError(HttpStatusCode statusCode, string message)
        {
            throw new HttpResponseException(new HttpResponseMessage(statusCode)
            {
                ReasonPhrase = message,
                Content = new StringContent(message)
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        protected void ThrowHttpError(HttpStatusCode statusCode, string message, string details)
        {
            throw new HttpResponseException(new HttpResponseMessage(statusCode)
            {
                ReasonPhrase = message,
                Content = new StringContent(details)
            });
        }
    }
}

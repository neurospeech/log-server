using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogServer.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class RequiresLoginAttribute: ActionFilterAttribute
    {

        private static bool? RequiresHTTPS = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (RequiresHTTPS == null) {
                RequiresHTTPS = System.Web.Configuration.WebConfigurationManager.AppSettings["RequiresHTTPS"] == "true";
            }

            var request = filterContext.HttpContext.Request;

            if (RequiresHTTPS.Value) {
                
                if (!request.IsSecureConnection) {

                    var uri = new UriBuilder(request.Url);
                    uri.Scheme = "https";
                    filterContext.Result = new RedirectResult(uri.ToString());
                    return;
                }
            }

            var isAuthenticated = filterContext.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated) {
                var uri = request.RawUrl;
                filterContext.Result = new RedirectResult("/user/login?redirectUrl=" + Uri.EscapeUriString(uri) );
                return;
            }

        }

    }
}
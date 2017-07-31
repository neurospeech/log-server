using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LogServer.Web
{

    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            try
            {
                // Web API configuration and services

                // Web API routes
                config.MapHttpAttributeRoutes();

                //config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
                
                config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter());

                //config.Routes.MapHttpRoute(
                //    name: "DefaultApi",
                //    routeTemplate: "api/{controller}/{id}",
                //    defaults: new { id = RouteParameter.Optional }
                //);

                
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.Fail(ex.Message, ex.ToString());
            }
        }
    }
}

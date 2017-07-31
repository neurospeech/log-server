using System.Web.Mvc;

namespace LogServer.Web.Areas.Api
{

    /// <summary>
    /// 
    /// </summary>
    public class ApiAreaRegistration : AreaRegistration 
    {

        /// <summary>
        /// 
        /// </summary>
        public override string AreaName 
        {
            get 
            {
                return "Api";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Api_default",
                "Api/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
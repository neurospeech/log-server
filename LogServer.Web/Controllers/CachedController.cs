using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LogServer.Web.Controllers
{
    /// <summary>
    /// Provides caching of resources, any url passed through this route will be
    /// cached for provided TimeSpan
    /// </summary>

    public class CachedController: Controller
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        [Route("cached/{version}/{*all}")]
        public ActionResult Get(string all) {


            string filePath = Server.MapPath("/" + all);

            if (!System.IO.File.Exists(filePath)) {
                throw new System.IO.FileNotFoundException($"all= {all}");
            }

            string mimeType = MimeMapping.GetMimeMapping(filePath);

            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetSlidingExpiration(true);
            Response.Cache.SetMaxAge(TimeSpan.FromDays(30));
            return File(filePath, mimeType);
        }


    }
}
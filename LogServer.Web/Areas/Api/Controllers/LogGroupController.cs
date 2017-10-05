using LogServer.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LogServer.Web.Areas.Api.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class LogGroupController : ApiDbController
    {

        /// <summary>
        /// Retrieve log groups of current application
        /// </summary>
        /// <param name="id">App Id</param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="user"></param>
        /// <param name="date"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [Route("api/apps/{id}/groups")]
        public async Task<IEnumerable<LogGroup>> Get(
            long id,
            int start = 0,
            int size = 50,
            string user=null,
            string date=null,
            string orderBy = "LastTime desc"
            )
        {
            var q = DB.LogGroups.Where(x => x.AppID == id);
            switch (orderBy?.ToLower())
            {
                case "lasttime desc":
                    q = q.OrderByDescending(x => x.LastTime);
                    break;
            }
            
            if (!string.IsNullOrWhiteSpace(user))
            {
                q = q.Where( x=>x.LogItems.Any( li => li.User.Contains(user) ) );
            }

            if (date!=null)
            {
                DateTime parsedDate = Convert.ToDateTime(date);
                q = q.Where(x => x.LogItems.Any(li => (li.Time == parsedDate)));

            }
            if (start > 0) {
                q = q.Skip(start);
            }
            
            q = q.Take(size);

            return await q.ToListAsync();
        }



    }

}
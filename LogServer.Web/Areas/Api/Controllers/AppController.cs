using LogServer.Web.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LogServer.Web.Areas.Api.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Route("api/apps")]
    [Authorize]
    public class AppController : ApiDbController
    {




        /// <summary>
        /// Get all apps
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="namesOnly"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AppInfo>> Get(bool namesOnly = false, bool isDeleted = false) {

            if (namesOnly) {
                return await DB.Apps.Where(x => x.IsDeleted == isDeleted).Select(x => new AppInfo {
                    Name = x.Name,
                    AppID = x.AppID
                }).ToListAsync();
            }

            string host = Request?.RequestUri?.DnsSafeHost;
            int port = Request?.RequestUri?.Port ?? 80;
            string protocol = Request?.RequestUri?.Scheme ?? "http";

            string url = $"{protocol}://{host}:{port}";

            var list =  await DB.Apps.Where(x => x.IsDeleted == isDeleted).Select(x=> new AppInfo {
                AppPushKey = x.AppPushKey,
                AppID = x.AppID,
                Name = x.Name,
                Platform = x.Platform,
                Total = x.LogGroups.Count(),
                LastUpdate = x.LogGroups.OrderByDescending(g=>g.LastTime).Select(g=>g.LastTime).FirstOrDefault()
            }).ToListAsync();
            foreach (var item in list) {
                item.PushUrl = $"{url}/api/logs/{item.AppID}/{item.AppPushKey}";
            }
            return list;
        }

        /// <summary>
        /// Resets push key...
        /// </summary>
        /// <param name="id">App id</param>
        /// <returns></returns>
        [Route("api/apps/{id}")]
        public async Task<App> Patch(long id) {
            var app = await DB.Apps.FirstOrDefaultAsync(x => x.AppID == id);
            if (app == null)
                ThrowHttpError(HttpStatusCode.NotFound, "App not found");
            app.AppPushKey = GenerateKey();
            await DB.SaveChangesAsync();
            return app;
        }


        /// <summary>
        /// Create or update the app
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<App> Put(App model) {

            var app = await DB.Apps.FirstOrDefaultAsync(x => x.AppID == model.AppID);
            if (app == null)
            {
                app = model;
                if (string.IsNullOrWhiteSpace(app.AppPushKey))
                {
                    app.AppPushKey = GenerateKey();
                }
                DB.Apps.Add(app);
            }
            else {
                app.Name = model.Name;
                app.IsDeleted = model.IsDeleted;
                app.Platform = model.Platform;
            }
            await DB.SaveChangesAsync();
            return app;
        }

        private static string GenerateKey()
        {
            return string.Join("", Guid.NewGuid().ToByteArray().Select(x => x.ToString("X2"))) +
                                    string.Join("", Guid.NewGuid().ToByteArray().Select(x => x.ToString("X2")));
        }

        /// <summary>
        /// Delete the app
        /// </summary>
        /// <param name="id">App id</param>
        /// <returns></returns>
        [Route("api/apps/{id}")]
        public async Task<IHttpActionResult> Delete(long id) {
            var app = await DB.Apps.FirstOrDefaultAsync(x => x.AppID == id);
            if (app == null) {
                ThrowHttpError(HttpStatusCode.NotFound, "App not found");
            }
            DB.Apps.Remove(app);
            await DB.SaveChangesAsync();
            return Ok();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public long AppID { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppPushKey { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdate { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Platform { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string PushUrl { get; internal set; }


        /// <summary>
        /// 
        /// </summary>
        public int Total { get; internal set; }
    }
}

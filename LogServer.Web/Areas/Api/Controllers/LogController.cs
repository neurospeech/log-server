using LogServer.Web.Models;
using Newtonsoft.Json;
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
    public class LogController : ApiDbController
    {


        /// <summary>
        /// Get all logs under group
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        [Route("api/logs/{groupID}")]
        [Authorize]
        public async Task<IEnumerable<LogItem>> Get(
            long groupID, 
            int start = 0, 
            int size = 50,
            string orderBy = "Time desc"
            ) {
            var q = DB.LogItems.Where(x => x.LogGroupID == groupID);
            switch (orderBy?.ToLower()) {
                case "time desc":
                    q = q.OrderByDescending(x => x.Time);
                    break;
            }

            if (start > 0)
            {
                q = q.Skip(0);
            }

            q = q.Take(size);

            return await q.ToListAsync();
        }



        /// <summary>
        /// Cleans old logs...
        /// </summary>
        /// <returns></returns>
        [Route("api/logs/clean")]
        public async Task<string> Clean() {
            int daysToKeep = await DB.GetConfigurationAsync<int>("LogItems.KeepHistoryDays", 30);

            DateTime historyMax = DateTime.UtcNow.AddDays(-daysToKeep);

            DB.Database.ExecuteSqlCommand("DELETE FROM LogItems WHERE Time<{0}", historyMax);
            DB.Database.ExecuteSqlCommand("UPDATE LogGroups SET " +
                "Total=(SELECT COUNT(*) FROM LogItems WHERE LogItems.LogGroupID = LogGroups.LogGroupID)," +
                "TotalUsers=(SELECT COUNT(*) FROM (SELECT LogItems.[User] FROM LogItems WHERE LogItems.LogGroupID = LogGroups.LogGroupID GROUP BY LogItems.[User]) as A)" + 
                "");
            

            return "Cleaned";
        }
        

        /// <summary>
        /// Insert new log entry for given application
        /// </summary>
        /// <param name="id">App Id</param>
        /// <param name="key">App push key</param>
        /// <param name="model">Log information</param>
        /// <param name="selfLog"></param>
        /// <returns></returns>
        [Route("api/logs/{id}/{key}")]
        public async Task<IHttpActionResult> Put(long id, string key, LogInfo model, bool selfLog = true) {

            if (!await DB.Apps.AnyAsync(x => x.AppID == id && x.AppPushKey == key)) {
                ThrowHttpError(HttpStatusCode.Forbidden, "Access denied...");
            }

            try
            {
                string title = model.Title;
                if (title.Length > 200)
                {
                    title = title.Substring(0, 200);
                }

                DateTime now = DateTime.UtcNow;

                var g = await DB.LogGroups.FirstOrDefaultAsync(x => x.AppID == id && x.Title == title);
                if (g == null)
                {
                    g = new Web.Models.LogGroup
                    {
                        AppID = id,
                        Title = title,
                        Status = "New",
                        StartTime = now,
                        LastTime = now,
                        LastUpdate = now,
                        Version = model.AppVersion
                    };
                    DB.LogGroups.Add(g);
                    await DB.SaveChangesAsync();
                }

                var av = await DB.AppVersions.FirstOrDefaultAsync(x => x.AppID == id && x.Version == model.AppVersion);
                if (av == null)
                {
                    av = new Web.Models.AppVersion
                    {
                        AppID = id,
                        Version = model.AppVersion
                    };
                    DB.AppVersions.Add(av);
                }

                g.LastTime = now;
                g.LastUpdate = now;
                g.Version = model.AppVersion;

                var item = new LogItem
                {
                    Platform = model.Platform,
                    Device = model.Device,
                    PlatformVersion = model.PlatformVersion,
                    Application = model.Application,
                    Detail = model.Detail,
                    Title = model.Title,
                    LogGroup = g,
                    LogType = model.LogType.ToString(),
                    Tag = model.Tag,
                    Time = now,
                    User = model.User,
                    UserAgent = model.UserAgent,
                    UserIPAddress = model.UserIPAddress,
                    Method = model.Method,
                    Url = model.Url
                };

                if (item.UserAgent != null) {
                    try
                    {
                        var p = UAParser.Parser.GetDefault();
                        var c = p.Parse(item.UserAgent);

                        item.Device = c.Device?.ToString() ?? "PC";
                        item.Application = $"{c.UA.Family} ({c.UA.Major}.{c.UA.Minor})";
                        item.Platform = c.OS.Family;
                        item.PlatformVersion = $"{c.OS.Major}.{c.OS.Minor}.{c.OS.Patch}";
                    }
                    catch(Exception ex) {
                        item.Detail += "\r\n" + ex.ToString();
                    }
                }

                DB.LogItems.Add(item);

                g.Total++;

                await DB.SaveChangesAsync();

                g.TotalUsers = await DB.LogItems.Where(x => x.LogGroupID == g.LogGroupID).GroupBy(x => x.User).CountAsync();

                await DB.SaveChangesAsync();
            }
            catch (Exception ex) {
                if (selfLog)
                {
                    var c = new LogController();
                    return await c.Put(id, key, new Controllers.LogInfo
                    {
                        Application = "Logger",
                        AppVersion = "1.0",
                        Title = ex.Message,
                        Detail = ex.ToString(),
                        LogType = LogType.Error,
                        Platform = "Logger",
                        PlatformVersion = "1.0",
                        Device = "API",
                        Tag = JsonConvert.SerializeObject(model)
                    }, false);
                }
                else {
                    throw;
                }
            }
            return Ok();
            
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class LogInfo {

        /// <summary>
        /// 
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PlatformVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// User Agent information of the user affected
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// IP Address of user affected
        /// </summary>
        public string UserIPAddress { get; set; }

        /// <summary>
        /// Detaileds of the log
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Short message about log
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Error, Warning, Info
        /// </summary>
        public LogType LogType { get; set; }

        /// <summary>
        /// User inormation such as user name and role etc
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// JSON Formatted text value for any other data
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Url for the request...
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// GET/POST/PUT/DELETE  etc...
        /// </summary>
        public string Method { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public enum LogType {
        /// <summary>
        /// 
        /// </summary>
        Info,
        /// <summary>
        /// 
        /// </summary>
        Warning,
        /// <summary>
        /// 
        /// </summary>
        Error
    }
}

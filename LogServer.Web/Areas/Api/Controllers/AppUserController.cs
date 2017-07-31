using LogServer.Web.Models;
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
    /// Get all users having access to the app
    /// </summary>
    [Route("api/apps/{id}/users")]
    [Authorize]
    public class AppUserController : ApiDbController
    {

        /// <summary>
        /// Get users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AppUserInfo>> Get(long id) {
            return await DB.AppUsers.Where(x => x.AppID == id).Select(x=> new AppUserInfo {
                User = x.WebUser
            }).ToListAsync();
        }


        /// <summary>
        /// Grant access for this app
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Put(long id, AppUserPut model) {
            var user = await DB.AppUsers.FirstOrDefaultAsync(x => x.AppID == id && x.Username == model.Username);
            if (user == null) {
                user = new AppUser {
                    AppID = id,
                    Username = model.Username
                };
                DB.AppUsers.Add(user);
                await DB.SaveChangesAsync();
            }
            return Ok();
        }


        /// <summary>
        /// Delete user form this app
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Delete(long id, AppUserPut model) {
            var user = await DB.AppUsers.FirstOrDefaultAsync(x => x.AppID == id && x.Username == model.Username);
            if (user == null)
                ThrowHttpError(HttpStatusCode.NotFound, "User not found in this app");
            DB.AppUsers.Remove(user);
            await DB.SaveChangesAsync();
            return Ok();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AppUserInfo {

        /// <summary>
        /// 
        /// </summary>
        public WebUser User { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AppUserPut {

        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }
    }
}


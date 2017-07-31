using LogServer.Web.Areas.Api.Models;
using LogServer.Web.Models;
using LogServer.Web.Services;
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
    [Route("api/users")]
    [Authorize]
    public class UserController : ApiDbController
    {

        /// <summary>
        /// List all users of exception server
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<WebUser>> Get() {
            IQueryable<WebUser> q = DB.WebUsers;
            if (!IsUserAdmin) {
                q = q.Where(x => x.Username == Username);
            }
            return await q.ToListAsync();
        }

        /// <summary>
        /// Add or update user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.NotAcceptable, "You must have atleast one administrator")]
        [SwaggerResponse(HttpStatusCode.Forbidden,"Access denied")]
        public async Task<WebUser> Put(WebUser model) {

            if (!IsUserAdmin)
                ThrowHttpError(HttpStatusCode.Forbidden, "Access denied");

            using (var transaction = DB.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                var hashService = Get<IHashService>();

                var user = await DB.WebUsers.FirstOrDefaultAsync(x => x.Username == model.Username);
                if (user == null)
                {
                    user = model;
                    user.IsActive = true;
                    DB.WebUsers.Add(user);
                }

                user.EmailAddress = model.EmailAddress;
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    user.PasswordHash = hashService.Hash(model.Password);
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Password = null;
                user.IsAdmin = model.IsAdmin;
                user.IsActive = model.IsActive;

                await DB.SaveChangesAsync();

                if (!await DB.WebUsers.AnyAsync(x => x.IsAdmin)) {
                    ThrowHttpError(HttpStatusCode.NotAcceptable, "You must have atleast one administrator");
                }

                transaction.Commit();

                return user;
            }
        }

        /// <summary>
        /// Delete user from system
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.NotFound,"User not found")]
        [SwaggerResponse(HttpStatusCode.MethodNotAllowed, "You cannot delete only one administrator user")]
        [SwaggerResponse(HttpStatusCode.NotAcceptable, "Remove user from all applications")]
        [SwaggerResponse(HttpStatusCode.Forbidden, "Access denied")]
        public async Task<IHttpActionResult> Delete(string userName) {

            if (!IsUserAdmin)
                ThrowHttpError(HttpStatusCode.Forbidden, "Access denied");


            var user = await DB.WebUsers.FirstOrDefaultAsync(x => x.Username == userName);
            if (user == null) {
                ThrowHttpError(HttpStatusCode.NotFound, "User not found");
            }

            if (await DB.AppUsers.AnyAsync(x => x.Username == userName)) {
                ThrowHttpError(HttpStatusCode.NotAcceptable, "Remove user from all applications");
            }

            if (user.IsAdmin)
            {
                if (await DB.WebUsers.CountAsync() > 1)
                {
                    DB.WebUsers.Remove(user);
                }
                else {
                    ThrowHttpError(HttpStatusCode.MethodNotAllowed, "You cannot delete only one administrator user");
                }
            }

            await DB.SaveChangesAsync();

            return Ok();
        }


    }
}

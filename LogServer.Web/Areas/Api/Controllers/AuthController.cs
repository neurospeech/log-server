using LogServer.Web.Areas.Api.Models;
using LogServer.Web.Services;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LogServer.Web.Areas.Api.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    [Route("api/auth")]
    public class AuthController: ApiDbController
    {

        /// <summary>
        /// Login with given username/password, this will set forms authentication cookie
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.NotFound, "User not found")]
        [SwaggerResponse(HttpStatusCode.NotAcceptable, "Invalid password")]
        [SwaggerResponse(HttpStatusCode.Forbidden, "Access denied")]
        public async Task<IHttpActionResult> Put(LoginModel model)
        {


            var hashService = Get<IHashService>();
            var authService = Get<IAuthService>();

            var user = await DB.WebUsers.FirstOrDefaultAsync(x => x.Username == model.Username);
            if (user == null)
                ThrowHttpError(HttpStatusCode.NotFound, "User not found");

            if (user.PasswordHash != hashService.Hash(model.Password))
                ThrowHttpError(HttpStatusCode.NotAcceptable, "Invalid password");

            if (!user.IsActive)
                ThrowHttpError(HttpStatusCode.Forbidden, "Access denied");

            await authService.AuthorizeAsync(model.Username, user.IsAdmin , model.RememberMe);

            return Created("", "Login successful");

        }


        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IHttpActionResult> Delete()
        {
            var authService = Get<IAuthService>();

            await authService.SignoutAsync();

            return Ok();
        }


    }
}
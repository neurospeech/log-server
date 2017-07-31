using NeuroSpeech.CoreDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace LogServer.Web.Services
{

    /// <summary>
    /// 
    /// </summary>
    public interface IAuthService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="remember"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        Task AuthorizeAsync(string username, bool isAdmin, bool remember);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task SignoutAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetUsername(HttpRequestMessage request, IPrincipal user);
    }

    /// <summary>
    /// 
    /// </summary>
    [DIGlobal(typeof(IAuthService))]
    public class CookieAuthService : IAuthService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="remember"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public Task AuthorizeAsync(string username, bool isAdmin, bool remember)
        {
            FormsAuthentication.SetAuthCookie(username, remember);
            return Task.CompletedTask;
        }

        private string GetAuthHeader(HttpRequestMessage request)
        {
            string _authHeader = null;
            IEnumerable<string> values;
            if (request.Headers.TryGetValues(Areas.Api.Controllers.ApiDbController.ApiKeyHeader, out values))
            {
                _authHeader = string.Join("", values);
                if (string.IsNullOrWhiteSpace(_authHeader))
                {
                    _authHeader = null;
                }
            }
            return _authHeader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetUsername(HttpRequestMessage request, IPrincipal user)
        {
            var cache = HttpRuntime.Cache;

            string a = GetAuthHeader(request);
            if (a == null)
            {

                a = GetFormsCookieHeader(request, user);
                if (a == null)
                {
                    throw new InvalidOperationException("Auth Key not specified");
                }
                return a;
            }

            string _username = (String)cache.Get(a);
            if (_username != null)
                return _username;



            var ticket = FormsAuthentication.Decrypt(a);
            _username = ticket.Name;
            cache.Add(
                a,
                _username,
                null,
                DateTime.UtcNow.AddMinutes(10),
                System.Web.Caching.Cache.NoSlidingExpiration,
                System.Web.Caching.CacheItemPriority.Normal,
                null);

            return _username;
        }

        private string GetFormsCookieHeader(HttpRequestMessage request, IPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
                return user.Identity.Name;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task SignoutAsync()
        {
            FormsAuthentication.SignOut();
            return Task.CompletedTask;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class MemoryAuthService : IAuthService
    {
        private static string _username = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="remember"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public Task AuthorizeAsync(string username, bool isAdmin , bool remember)
        {
            _username = username;

            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetUsername(HttpRequestMessage request, IPrincipal user)
        {
            return _username;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task SignoutAsync()
        {
            _username = null;
            return Task.CompletedTask;
        }
    }
}
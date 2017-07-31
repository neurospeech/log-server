using LogServer.Web.Services;
using NeuroSpeech.CoreDI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Principal;
using System.Runtime.Remoting.Messaging;

namespace UnitTests.Services
{
    [DIGlobal(typeof(IAuthService))]
    public class MemoryAuthService : IAuthService
    {

        private static string _username
        {
            get
            {

                return (string)CallContext.LogicalGetData("MemoryAuthService.Username");
            }
            set
            {
                CallContext.LogicalSetData("MemoryAuthService.Username", value);
            }
        }

        public Task AuthorizeAsync(string username, bool isAdmin, bool remember)
        {
            _username = username;

            return Task.CompletedTask;
        }

        public string GetUsername(HttpRequestMessage request, IPrincipal user)
        {
            return _username;
        }

        public Task SignoutAsync()
        {
            _username = null;
            return Task.CompletedTask;
        }
    }
}

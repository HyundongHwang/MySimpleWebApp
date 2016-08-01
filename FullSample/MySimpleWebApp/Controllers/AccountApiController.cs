using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System.Net;

namespace MySimpleWebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountApiController : ApiController
    {
        // POST api/Account/Logout
        [Route("logout")]
        public IHttpActionResult GetLogout()
        {
            var authManager = this.Request.GetOwinContext().Authentication;
            authManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }
    }
}
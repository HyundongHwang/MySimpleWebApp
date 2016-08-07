using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MySimpleWebApp.Controllers
{
    [RoutePrefix("api/tests")]

    public class TestsApiController : ApiController
    {
        [Route("cookie")]
        public string GetCookie(string cookieStr)
        {
            if (string.IsNullOrWhiteSpace(cookieStr))
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = new StringContent(cookieStr),
                });
            }

            return cookieStr;
        }
    }
}
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MySimpleWebApp.Filters
{
    public class MyMvcExceptionFilterAttribute : IExceptionFilter
    {
        public void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            Trace.TraceError($@"
MVC EXCEPTION!!!
{filterContext.RequestContext.HttpContext.Request.Url}
{filterContext.Exception.ToString()}
            ");
        }
    }

}
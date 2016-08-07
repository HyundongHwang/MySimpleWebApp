using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace MySimpleWebApp.Filters
{
    public class MyWebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Trace.TraceError($@"
WEBAPI EXCEPTION!!!
{actionExecutedContext.Request.RequestUri}
{actionExecutedContext.Exception.ToString()}
            ");
        }
    }

}
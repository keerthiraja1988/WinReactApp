namespace WinReactApp.UserAuth.Extensions.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    [ExcludeFromCodeCoverage]
    public class LoggingActionFilter : IAsyncActionFilter
    {
        private Logger _nlogger = LogManager.GetCurrentClassLogger(); // creates a logger using the class name

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string apiVersion = context.HttpContext.Request.Query["api-version"];

            if (!string.IsNullOrEmpty(apiVersion))
            {
                context.HttpContext.Response.Headers.Add("X-Api-Version", apiVersion);
            }
            else
            {
                context.HttpContext.Response.Headers.Add("X-Api-Version", "1.1");
            }

            context.HttpContext.Response.Headers.Add("X-Default-Api-Version", "1.1");
            context.HttpContext.Response.Headers.Add("X-Request-Id", context.HttpContext.TraceIdentifier);
            context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.HttpContext.Response.Headers.Add("X-Frame-Options", "DENY");
            context.HttpContext.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

            var localTimeZone = System.TimeZoneInfo.Local;
            string responseTimeZome = localTimeZone.Id + " - " + localTimeZone.DisplayName;

            context.HttpContext.Response.Headers.Add("_RequestTime", DateTime.Now.ToString());
            context.HttpContext.Response.Headers.Add("_Server-TimeZone", responseTimeZome);

            this._nlogger = LogManager.GetLogger(context.ActionDescriptor.DisplayName);

            try
            {
                this._nlogger.Info(
                     "Executing Method - " + context.ActionDescriptor.DisplayName);

                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    //var userAuthenticationModel = WebAppMVCExtensions.GetLoggedInUserDetails(context.HttpContext.User);
                }

                var resultContext = await next();

                if (resultContext.Exception == null)
                {
                    this._nlogger.Info(
                             "Successfuly Executed Method - " + context.ActionDescriptor.DisplayName);
                }
                else
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    this._nlogger.Error("Error Occured on Executing Method - " + resultContext.Exception.Message, resultContext.Exception);
#pragma warning restore CS0618 // Type or member is obsolete

                    StackTrace st = new StackTrace(resultContext.Exception, true);
                    StackFrame frame = st.GetFrame(0);
                    string fileName = frame.GetFileName();
                    string methodName = frame.GetMethod().Name;
                    int line = frame.GetFileLineNumber();
                    int col = frame.GetFileColumnNumber();

                    throw new Exception("Exception Thrown : "
                       + "\n  Type :    " + resultContext.Exception.GetType().Name
                       + "\n  Message : " + resultContext.Exception.Message
                       + "\n  Project : " + resultContext.Exception.Source
                       + "\n  Source : " + fileName + " | Method : " + methodName + " | Line :" + line.ToString() + " | " + col.ToString());
                }
            }
#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable CS0168 // Variable is declared but never used
            catch (Exception)
#pragma warning restore CS0168 // Variable is declared but never used
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            {
                throw;
            }
        }
    }

    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            context.Result = new JsonResult(exception.Message);

            context.ExceptionHandled = true;

            //context.HttpContext.Response.Headers.Add("X-Request-Id", context.HttpContext.TraceIdentifier);

            base.OnException(context);
        }
    }
}
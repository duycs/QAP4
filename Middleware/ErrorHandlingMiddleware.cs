using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using QAP4.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QAP4.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        //public ErrorHandlingMiddleware(RequestDelegate next)
        //{
        //    this.next = next;
        //}

        //public async Task Invoke(HttpContext context, string s /* other scoped dependencies */)
        //{
        //    if (AppConstants.getCode(s)!=0)
        //    {
        //        await (HandleExceptionAsync(context, s));
        //    }
        //}

        //private static Task HandleExceptionAsync(HttpContext context, string s)
        //{
        //    //var code = AppConstants.getCode(s);
        //    //var message = AppConstants.getMessage(s);

        //    var result = JsonConvert.SerializeObject(new { error = message });
        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = (int)code;
        //    return context.Response.WriteAsync(result);
        //}
    }
}

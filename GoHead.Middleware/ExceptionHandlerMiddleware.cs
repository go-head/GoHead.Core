using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GoHead.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, Correlation correlation)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, correlation);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, Correlation correlation)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (exception is ApplicationException) code = HttpStatusCode.BadRequest;
            //sempre o ultimo
            else if (exception is Exception) code = HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(new { error = exception.Message, correlationId = correlation.Id });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace GoHead.Middleware
{
    public class CorrelationHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private Correlation _correlation;

        public CorrelationHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, Correlation correlation)
        {
            _correlation = correlation;
            httpContext.Request.Headers.TryGetValue(Correlation.PARAMNAME, out StringValues correlationStr);
            SetCorrelationId(correlationStr);

            httpContext.Response.Headers.Add(Correlation.PARAMNAME, correlation.Id.ToString());

            return _next(httpContext);
        }

        private void SetCorrelationId(StringValues correlationStr)
        {
            Guid.TryParse(correlationStr, out Guid correlation);
            var correlationId = correlation == Guid.Empty ? Guid.NewGuid() : correlation;

            _correlation.SetId(correlationId);
        }
    }
}
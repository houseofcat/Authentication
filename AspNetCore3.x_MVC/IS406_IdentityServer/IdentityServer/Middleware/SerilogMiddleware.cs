using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;
using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Middleware
{
    public static class SerilogHttpContextExtensions
    {
        public static IApplicationBuilder UseSerilogHttpContextLogger(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SerilogHttpContextLogger>();
        }
    }

    public class SerilogHttpContextLogger
    {
        private readonly RequestDelegate _next;
        private ArrayPool<byte> SharedBytePool { get; } = ArrayPool<byte>.Shared;

        public SerilogHttpContextLogger(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var username = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : "anonymous";
            LogContext.PushProperty("UserName", username);
            LogContext.PushProperty("IP", httpContext.Connection.RemoteIpAddress.ToString());

            await LogRequestAsync(httpContext);
            await _next(httpContext);
            await LogResponseAsync(httpContext);
        }

        private const string RequestTemplate = "Request: {RequestMethod} {RequestPath}";
        private const string RequestTooLargeTemplate = "Request: {RequestMethod} {RequestPath} (Body Too Large)";

        private async Task LogRequestAsync(HttpContext httpContext)
        {
            if (httpContext.Request.ContentLength.HasValue
                && httpContext.Request.ContentLength.Value > 0
                && httpContext.Request.ContentLength.Value < 100_000)
            {
                // Reading Request Bodies // Logging Requests
                httpContext.Request.EnableBuffering(); // Enables us to read/reset the request body stream.

                var length = Convert.ToInt32(httpContext.Request.ContentLength.Value);
                var buffer = SharedBytePool.Rent(length);

                // Rented buffers often don't have exact length, but we know data length.
                await httpContext.Request.Body.ReadAsync(buffer, 0, length);

                // Reset Position
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);

                Log
                    .ForContext(
                        "RequestHeaders",
                        httpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                        destructureObjects: true)
                    .ForContext(
                        "RequestBody",
                        Encoding.UTF8.GetString(buffer))
                    .Information(
                        RequestTemplate,
                        httpContext.Request.Method,
                        httpContext.Request.Path);

                SharedBytePool.Return(buffer);
            }
            else
            {
                Log
                    .ForContext(
                        "RequestHeaders",
                        httpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                        destructureObjects: true)
                    .Information(
                        RequestTooLargeTemplate,
                        httpContext.Request.Method,
                        httpContext.Request.Path);
            }
        }

        private const string ResponseTemplate = "Response: {RequestMethod} {RequestPath} {StatusCode}";
        private const string ResponseTooLargeTemplate = "Response: {RequestMethod} {RequestPath} {StatusCode} (Body Too Large)";

        private async Task LogResponseAsync(HttpContext httpContext)
        {
            if (httpContext.Response.ContentLength.HasValue
                && httpContext.Response.ContentLength.Value > 0
                && httpContext.Response.ContentLength.Value < 100_000)
            {
                // Reset To Read
                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

                // Reset For Client
                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

                Log
                    .ForContext(
                        "ResponseBody",
                        responseBody)
                    .Information(
                        ResponseTemplate,
                        httpContext.Request.Method,
                        httpContext.Request.Path,
                        httpContext.Response.StatusCode);
            }
            else
            {
                Log
                    .Information(
                        ResponseTooLargeTemplate,
                        httpContext.Request.Method,
                        httpContext.Request.Path,
                        httpContext.Response.StatusCode);
            }
        }
    }
}

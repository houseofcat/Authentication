using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Serilog;
using Serilog.Context;
using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMvClient.Middleware
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
        private readonly bool LogRequestBody;
        private readonly bool LogResponseBody;

        public SerilogHttpContextLogger(RequestDelegate next, IConfiguration configuration)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            LogRequestBody = configuration.GetSection("Application").GetValue<bool>("LogRequestBody");
            LogResponseBody = configuration.GetSection("Application").GetValue<bool>("LogResponseBody");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            // If User Is Authenticated, get their username, else use "*" to denote anonymous.
            // The restrictions on creating users doesn't allow for the "*" in the username. You would want
            // to use something that contains characters only the system could use for logging. Using things 
            // like "anonymous" is a valid user name and could be confusing to you later on combing through the logs.
            var username = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : "*"; 
            LogContext.PushProperty("UserName", username);
            LogContext.PushProperty("IP", httpContext.Connection.RemoteIpAddress.ToString());
            LogContext.PushProperty("UserAgent", httpContext.Request.Headers["User-Agent"]);

            if (LogRequestBody) { await LogRequestBodyAsync(httpContext); }

            await _next(httpContext);

            if (LogResponseBody) { await LogResponseBodyAsync(httpContext); }
        }

        private const string RequestTemplate = "Middleware Request: {RequestMethod} {RequestPath}";
        private const string RequestTooLargeTemplate = "Middleware Request: {RequestMethod} {RequestPath} (Body Skipped)";

        private async Task LogRequestBodyAsync(HttpContext httpContext)
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
                        Encoding.UTF8.GetString(buffer, 0, length))
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

        private const string ResponseTemplate = "Middleware Response: {RequestMethod} {RequestPath} {StatusCode}";
        private const string ResponseTooLargeTemplate = "Middleware Response: {RequestMethod} {RequestPath} {StatusCode} (Body Skipped)";

        private async Task LogResponseBodyAsync(HttpContext httpContext)
        {
            if (httpContext.Response.ContentLength.HasValue
                && httpContext.Response.ContentLength.Value > 0
                && httpContext.Response.ContentLength.Value < 100_000)
            {
                // Reset To Read
                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

                // Careful here, without LeaveOpen, the StreamReader would close the underlying
                // stream on Dispose.
                using var streamReader = new StreamReader(httpContext.Response.Body, leaveOpen: true);
                var responseBody = await streamReader.ReadToEndAsync();

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

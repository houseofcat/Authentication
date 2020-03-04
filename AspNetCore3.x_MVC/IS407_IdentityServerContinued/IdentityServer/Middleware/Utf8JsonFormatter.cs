using Microsoft.AspNetCore.Mvc.Formatters;
using System.Threading.Tasks;
using Utf8Json;

namespace IdentityServer.Middleware
{
    public class JsonOutputFormatter : IOutputFormatter
    {
        const string ContentType = "application/json";

        private readonly IJsonFormatterResolver resolver;

        public JsonOutputFormatter() : this(null) { }

        public JsonOutputFormatter(IJsonFormatterResolver resolver)
        {
            this.resolver = resolver ?? JsonSerializer.DefaultResolver;
        }

        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return true;
        }

        public async Task WriteAsync(OutputFormatterWriteContext context)
        {
            context.HttpContext.Response.ContentType = ContentType;

            if (context.ObjectType == typeof(object))
            {
                await JsonSerializer
                    .NonGeneric
                    .SerializeAsync(
                        context.HttpContext.Response.Body,
                        context.Object,
                        resolver);
            }
            else
            {
                await JsonSerializer
                    .NonGeneric
                    .SerializeAsync(
                        context.ObjectType,
                        context.HttpContext.Response.Body,
                        context.Object,
                        resolver);
            }
        }
    }

    public class JsonInputFormatter : IInputFormatter
    {
        private readonly IJsonFormatterResolver resolver;

        public JsonInputFormatter() : this(null) { }

        public JsonInputFormatter(IJsonFormatterResolver jsonResolver)
        {
            resolver = jsonResolver ?? JsonSerializer.DefaultResolver;
        }


        public bool CanRead(InputFormatterContext context)
        {
            return true;
        }

        public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            var result = await JsonSerializer
                .NonGeneric
                .DeserializeAsync(
                    context.ModelType,
                    context.HttpContext.Request.Body,
                    resolver);

            return await InputFormatterResult
                .SuccessAsync(result);
        }
    }
}

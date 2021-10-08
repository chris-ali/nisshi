using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Nisshi.Infrastructure.Errors
{
    /// <summary>
    /// Invokes requests, handling exceptions by logging them and returning 
    /// error messages in a friendlier format for responses
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        private readonly IStringLocalizer<ErrorHandlingMiddleware> localizer;

        public ErrorHandlingMiddleware(
            RequestDelegate next, 
            ILogger<ErrorHandlingMiddleware> logger, 
            IStringLocalizer<ErrorHandlingMiddleware> localizer)
        {
            this.next = next;
            this.logger = logger;
            this.localizer = localizer;
        }

        public async Task Invoke(HttpContext context) 
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger, localizer);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, 
            Exception ex, 
            ILogger<ErrorHandlingMiddleware> logger, 
            IStringLocalizer<ErrorHandlingMiddleware> localizer)
        {
            string result = null;
            switch (ex) 
            {
                case RestException re:
                    context.Response.StatusCode = (int)re.Code;
                    result = JsonSerializer.Serialize(new { errors = localizer[re.Error?.ToString()].Value });
                    break;
                case Exception ey:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    logger.LogError(ey, "Unhandled Exception");
                    result = JsonSerializer.Serialize(new { errors = localizer["InternalServerError"].Value});
                    break;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result ?? "{}");
        }

    }
}
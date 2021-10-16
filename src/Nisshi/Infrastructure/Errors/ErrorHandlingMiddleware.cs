using System;
using System.Net;
using System.Security.Authentication;
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
                case InvalidCredentialException ice:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { errors = localizer[ice.Message].Value });
                    break;
                case AuthenticationException ae:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize(new { errors = localizer[ae.Message].Value });
                    break;
                case DomainException de:
                    int status = de.MessageCode == Message.ItemDoesNotExist ? 
                        (int)HttpStatusCode.NotFound : (int)HttpStatusCode.BadRequest;
                    context.Response.StatusCode = status;
                    result = JsonSerializer.Serialize(new { errors = $"{de.EntityType?.Name} {localizer[de.MessageCode.ToString()].Value}" });
                    break;
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
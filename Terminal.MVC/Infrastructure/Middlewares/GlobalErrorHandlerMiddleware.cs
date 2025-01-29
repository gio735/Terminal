using Serilog;
using System.Net;
using System.Text.Json;
using Terminal.Infrastructure;
using Terminal.MVC.Infrastructure.Localizations;

namespace Terminal.MVC.Infrastructure.Middlewares
{
    public class GlobalErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;
        public GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            foreach (var header in headers)
            {
                await Console.Out.WriteLineAsync(header.Key + "    " + header.Value);
            }
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                string result;
                response.ContentType = "application/json";

                switch (error)
                {
                    case InexistentEntityException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        result = JsonSerializer.Serialize(new { message = ErrorMessages.NotFound });
                        break;
                    case AttemptToUseDeletedEntityException:
                    case UnauthorizedAccessException:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        result = JsonSerializer.Serialize(new { message = ErrorMessages.Forbidden });
                        break;
                    case ExistingUsernameException:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        result = JsonSerializer.Serialize(new { message = ErrorMessages.ExistingMail });
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        result = JsonSerializer.Serialize(new { message = ErrorMessages.InternalError });
                        break;
                };
                await response.WriteAsync(result);
                _logger.LogError(error.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}

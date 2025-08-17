using RealEstate.Api.ViewModels;
using RealEstate.Domain.Exceptions;

namespace RealEstate.Api.Middleware
{
    // Take-Home Assignment: 
    // o	Implement error handling (e.g., 404 for not found, 400 for invalid input) with JSON responses, plus logging for debugging.
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorResponse = new ErrorResponse(exception.Message);

            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case UnauthorizedAccessException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    _logger.LogWarning(exception, "Unauthorized access");
                    break;
                case RequestArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    _logger.LogWarning(exception, "Bad request");
                    break;
                case NotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    _logger.LogInformation(exception, "Not found");
                    break;
                case UserAlreadyRegisteredException:
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    _logger.LogWarning(exception, "Conflict");
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    _logger.LogError(exception, "Unhandled exception");
                    break;
            }

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            return app;
        }
    }
}

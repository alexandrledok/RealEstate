using System.Collections.Concurrent;
using System.Net;

namespace RealEstate.Api.Middleware
{
    public class RateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, (DateTime Timestamp, int Count)> Requests = new();
        private const int LIMIT = 60;
        private static readonly TimeSpan TIME_WINDOW = TimeSpan.FromMinutes(1);

        public RateLimiterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var now = DateTime.UtcNow;

            var (timestamp, count) = Requests.GetOrAdd(ip, _ => (now, 0));

            if (now - timestamp < TIME_WINDOW)
            {
                if (count >= LIMIT)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    await context.Response.WriteAsJsonAsync(new { error = "Rate limit exceeded" });
                    return;
                }
                Requests[ip] = (timestamp, count + 1);
            }
            else
            {
                Requests[ip] = (now, 1);
            }

            await _next(context);
        }
    }

    public static class RateLimiterMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomRateLimiter(this IApplicationBuilder app)
        {
            app.UseMiddleware<RateLimiterMiddleware>();
            return app;
        }
    }
}

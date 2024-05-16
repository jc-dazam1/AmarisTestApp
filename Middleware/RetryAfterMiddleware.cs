using System.Net;

namespace AmarisTestApp.Middleware
{
    public class RetryAfterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RetryAfterMiddleware> _logger;

        public RetryAfterMiddleware(RequestDelegate next, ILogger<RetryAfterMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.TooManyRequests)
            {
                var retryAfterHeader = context.Response.Headers["Retry-After"];

                if (!string.IsNullOrEmpty(retryAfterHeader))
                {
                    if (int.TryParse(retryAfterHeader, out int retryAfterSeconds))
                    {
                        _logger.LogInformation($"Too Many Requests received. Waiting for {retryAfterSeconds} seconds before retrying.");
                        await Task.Delay(TimeSpan.FromSeconds(retryAfterSeconds));
                        context.Response.StatusCode = (int)HttpStatusCode.OK; // Retry the request
                        await _next(context);
                    }
                    else
                    {
                        _logger.LogWarning("Retry-After header value is not a valid integer.");
                    }
                }
                else
                {
                    _logger.LogWarning("Retry-After header not found.");
                }
            }
        }
    }
    public static class RetryAfterMiddlewareExtensions
    {
        public static IApplicationBuilder UseRetryAfterMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RetryAfterMiddleware>();
        }
    }
}

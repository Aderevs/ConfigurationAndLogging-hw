namespace Task3
{
    public class ExceptionLoggingMiddleware
    {
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;
        public ExceptionLoggingMiddleware(ILogger<ExceptionLoggingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
    public static class ExceptionLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionLoggingMiddleware>();
        }
    }
}

using System.Net;

namespace WebApiStartup.Middlewares
{
    public class GlobalExceptionhandlerMiddleware
    {
        private readonly ILogger<GlobalExceptionhandlerMiddleware> _logger;
        private readonly RequestDelegate _next;  // RequestDelegate (A function that can process an HTTP request.)

        public GlobalExceptionhandlerMiddleware(ILogger<GlobalExceptionhandlerMiddleware> logger, RequestDelegate next)
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
                var errorId = Guid.NewGuid().ToString();
                _logger.LogError(ex, $"{errorId}: {ex.Message}");

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Someting went wrong",
                };
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}

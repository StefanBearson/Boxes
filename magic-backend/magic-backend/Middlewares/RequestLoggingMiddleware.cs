namespace magic_backend.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    
    public RequestLoggingMiddleware(RequestDelegate next , ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"Request: {context.Request.Path} {context.Request.QueryString} {context.Request.Method}");
        await _next(context);
        _logger.LogInformation($"Finished processing {context.Request.Path} {context.Request.QueryString} {context.Request.Method}");
    }
}
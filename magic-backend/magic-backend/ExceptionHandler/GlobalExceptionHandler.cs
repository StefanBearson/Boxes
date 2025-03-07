using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace magic_backend.ExceptionHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "no magic today",
            Instance = httpContext.Request.Path,
            Status = httpContext.Response.StatusCode,
            Detail = exception.Message,
            Type = httpContext.Request.Method,
        };
        
        logger.LogError($"{problemDetails.Instance} {problemDetails.Status} {problemDetails.Detail} {exception.Message}");
        await httpContext.Response.WriteAsJsonAsync(problemDetails);
        
        return true;
    }
}
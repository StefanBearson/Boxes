using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace magic_backend.ExceptionHandler;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();
        problemDetails.Instance = httpContext.Request.Path;
        problemDetails.Status = httpContext.Response.StatusCode;
        
        logger.LogError($"{problemDetails.Instance} {problemDetails.Status} {problemDetails.Detail} {exception.Message}");;
        await httpContext.Response.WriteAsJsonAsync(problemDetails);

        return true;
    }
}
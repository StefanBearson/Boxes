using FluentValidation;

namespace magic_backend.Filters;

public class ValidationFilter<TRequest> : IEndpointFilter
{
    private readonly IValidator<TRequest> _validator;
    private ILogger<ValidationFilter<TRequest>> _logger;
    
    public ValidationFilter(IValidator<TRequest> validator, ILogger<ValidationFilter<TRequest>> logger)
    {
        _validator = validator;
        _logger = logger;
    }
    
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context, 
        EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<TRequest>().First();
        var result = await _validator.ValidateAsync(request, context.HttpContext.RequestAborted);
        
        if(!result.IsValid)
        {
            _logger.LogWarning("Validation failed for {Request}. Errors: {@Errors}", request, result.Errors);
            return TypedResults.ValidationProblem(result.ToDictionary());
        }

        return await next(context);
    }
}
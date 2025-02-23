using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Application.Exceptions;

namespace OrderManagement.Api.Web.Handlers;

public class KnownExceptionsHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<HttpContext, Exception, CancellationToken, Task>> _exceptionHandlers = new()
    {
        { typeof(ValidationException), HandleValidationException },
        { typeof(EntityNotFoundException), HandleEntityNotFoundException }
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionType = exception.GetType();

        if (!_exceptionHandlers.TryGetValue(exceptionType, out var handlerFunc)) 
            return false;
        
        await handlerFunc.Invoke(httpContext, exception, cancellationToken);
        
        return true;
    }

    private static async Task HandleEntityNotFoundException(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails
        {
            Title = exception.Message,
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
        }, cancellationToken);
    }

    private static async Task HandleValidationException(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
    {
        var exception = (ValidationException)ex;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors)
        {
            Title = exception.Message,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        }, cancellationToken);
    }
}
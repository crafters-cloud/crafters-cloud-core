using FluentValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.AspNetCore.Exceptions;

[PublicAPI]
internal sealed class CoreGlobalExceptionHandler(ILogger<CoreGlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        switch (exception)
        {
            case ValidationException validationException:
                var validationProblemDetails = httpContext.CreateValidationProblemDetails(validationException);
                await httpContext.WriteProblemDetails(validationProblemDetails, cancellationToken);
                break;
            default:
                var problemDetails = httpContext.CreateProblemDetails(exception);
                await httpContext.WriteProblemDetails(problemDetails, cancellationToken);
                break;
        }

        return true;
    }
}
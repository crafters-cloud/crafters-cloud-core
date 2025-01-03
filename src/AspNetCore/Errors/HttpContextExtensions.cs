using System.Diagnostics;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace CraftersCloud.Core.AspNetCore.Errors;

[PublicAPI]
public static class HttpContextExtensions
{
    public static ProblemDetails CreateProblemDetails(this HttpContext context, Exception exception)
    {
        var environment = context.Resolve<IHostEnvironment>();
        var errorDetail = environment.IsDevelopment()
            ? exception.Demystify().ToString()
            : "The instance value should be used to identify the problem when calling customer support";

        var problemDetails = new ProblemDetails
        {
            Title = "An unexpected error occurred!",
            Instance = context.Request.Path,
            Status = StatusCodes.Status500InternalServerError,
            Detail = errorDetail
        };

        return problemDetails;
    }

    public static ValidationProblemDetails CreateValidationProblemDetails(this HttpContext context,
        IEnumerable<ValidationFailure> validationFailures)
    {
        var problemDetails = new ValidationProblemDetails
        {
            Title = "Validation problem occurred!",
            Instance = context.Request.Path,
            Status = StatusCodes.Status400BadRequest,
            Detail = "Please refer to the errors property for additional details.",
            Errors = validationFailures.ToProblemDetailsErrors()
        };

        return problemDetails;
    }

    public static async Task WriteProblemDetails<T>(this HttpContext httpContext, T problemDetails,
        CancellationToken cancellationToken) where T : ProblemDetails
    {
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }

    public static IDictionary<string, string[]> ToProblemDetailsErrors(
        this IEnumerable<ValidationFailure> validationFailures) => validationFailures
        .GroupBy(x => x.PropertyName)
        .ToDictionary(
            group => group.Key,
            group => group.Select(g => g.ErrorMessage).ToArray());
}
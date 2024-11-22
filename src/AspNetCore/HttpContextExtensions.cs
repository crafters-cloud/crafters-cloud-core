using System.Diagnostics;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CraftersCloud.Core.AspNetCore;

[PublicAPI]
public static class HttpContextExtensions
{
    public static T Resolve<T>(this HttpContext httpContext) where T : notnull =>
        httpContext.RequestServices.GetRequiredService<T>();

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
        ValidationException validationException) =>
        CreateValidationProblemDetails(context, validationException.Errors);

    public static ValidationProblemDetails CreateValidationProblemDetails(this HttpContext context,
        IEnumerable<ValidationFailure> validationFailures) =>
        ValidationProblemDetailsMapper.CreateValidationProblemDetails(context.Request.Path, validationFailures);

    public static async Task WriteProblemDetails<T>(this HttpContext httpContext, T problemDetails,
        CancellationToken cancellationToken) where T : ProblemDetails
    {
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
    }
}
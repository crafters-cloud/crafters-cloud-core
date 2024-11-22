using CraftersCloud.Core.AspNetCore.Validation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CraftersCloud.Core.AspNetCore;

public static class ValidationProblemDetailsMapper
{
    public static ValidationProblemDetails CreateValidationProblemDetails(string instance,
        IEnumerable<ValidationFailure> validationFailures)
    {
        var problemDetails = new ValidationProblemDetails
        {
            Instance = instance,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://asp.net/core",
            Detail = "Please refer to the errors property for additional details.",
            Errors = validationFailures.ToProblemDetailsErrors()
        };

        return problemDetails;
    }
}
using CraftersCloud.Core.AspNetCore.Errors;
using CraftersCloud.Core.AspNetCore.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using HttpResults = Microsoft.AspNetCore.Http.Results;

namespace CraftersCloud.Core.AspNetCore.MinimalApi;

public class RequestValidatorFilter<T>(IEnumerable<IValidator<T>> validators) : IEndpointFilter
    where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T)) is not T model)
        {
            return HttpResults.BadRequest();
        }

        var failures = await validators.ValidateAllAsync(model);
        if (failures.Count == 0)
        {
            return await next(context);
        }

        var validationProblemDetails = context.HttpContext.CreateValidationProblemDetails(failures);
        return HttpResults.BadRequest(validationProblemDetails);
    }
}
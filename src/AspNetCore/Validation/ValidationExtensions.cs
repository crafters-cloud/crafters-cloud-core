using FluentValidation;
using FluentValidation.Results;

namespace CraftersCloud.Core.AspNetCore.Validation;

public static class ValidationExtensions
{
    public static async Task<List<ValidationFailure>> ValidateAllAsync<T>(this IEnumerable<IValidator<T>> validators,
        T model)
    {
        var results = await Task.WhenAll(validators
            .Select(async v => await v.ValidateAsync(model)));

        var failures = results
            .SelectMany(result => result.Errors)
            .ToList();
        return failures;
    }
}
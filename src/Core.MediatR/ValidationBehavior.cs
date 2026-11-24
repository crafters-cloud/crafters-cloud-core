using System.Reflection;
using System.Runtime.CompilerServices;
using CraftersCloud.Core.Results;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CraftersCloud.Core.MediatR;

/// <summary>
/// A behavior in the MediatR pipeline that validates incoming requests using FluentValidation validators.
/// </summary>
/// <typeparam name="TRequest">The type of request being validated. Must implement <see cref="IBaseRequest"/>.</typeparam>
/// <typeparam name="TResponse">The type of response produced by the request handler.</typeparam>
/// <remarks>
/// If validation fails and the response is a union with a <see cref="BadRequestResult"/> case
/// (e.g. <see cref="CreateResult{T}"/>, <see cref="UpdateResult"/>, <see cref="CommandResult{T,TError}"/>),
/// a <see cref="BadRequestResult"/> is returned. Otherwise a <see cref="ValidationException"/> is thrown.
/// </remarks>
[PublicAPI]
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
{
    // resolved once per closed generic type
    private static readonly ConstructorInfo? BadRequestConstructor =
        typeof(IUnion).IsAssignableFrom(typeof(TResponse))
            ? typeof(TResponse).GetConstructor([typeof(BadRequestResult)])
            : null;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var results = await Task.WhenAll(validators
            .Select(async v => await v.ValidateAsync(request, cancellationToken)));

        var failures = results
            .SelectMany(result => result.Errors)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        if (BadRequestConstructor == null)
        {
            throw new ValidationException(failures);
        }

        var badRequest = new BadRequestResult(failures.ToValidationErrors());
        return (TResponse) BadRequestConstructor.Invoke([badRequest]);
    }
}

[PublicAPI]
public static class ValidationFailureExtensions
{
    /// <summary>
    /// Converts FluentValidation failures to the library-independent <see cref="ValidationError"/>.
    /// </summary>
    public static IReadOnlyList<ValidationError> ToValidationErrors(this IEnumerable<ValidationFailure> failures) =>
        [.. failures.Select(f => new ValidationError(f.PropertyName, f.ErrorMessage, f.ErrorCode))];
}

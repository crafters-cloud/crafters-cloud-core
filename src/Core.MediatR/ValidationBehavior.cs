using CraftersCloud.Core.Results;
using FluentValidation;
using MediatR;

namespace CraftersCloud.Core.MediatR;

/// <summary>
/// A behavior in the MediatR pipeline that validates incoming requests using FluentValidation validators.
/// </summary>
/// <typeparam name="TRequest">The type of request being validated. Must implement <see cref="IBaseRequest"/>.</typeparam>
/// <typeparam name="TResponse">The type of response produced by the request handler.</typeparam>
/// <remarks>
/// If validation fails and the response type does not derive from <see cref="BadRequestResult"/>, a <see cref="ValidationException"/> is thrown.
/// If the response type does derive from <see cref="BadRequestResult"/>, a bad request result is returned with the validation errors.
/// </remarks>
[PublicAPI]
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
{
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

        if (!typeof(TResponse).IsDerivedFromOneOfType<BadRequestResult>())
        {
            throw new ValidationException(failures);
        }

        var invalidResult = new BadRequestResult(failures);
        return invalidResult.MapToOneOf<TResponse>();
    }
}
using CraftersCloud.Core.Results;
using FluentValidation;
using MediatR;

namespace CraftersCloud.Core.MediatR;

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
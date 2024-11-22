using CraftersCloud.Core.Results;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
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
            await next();
        }
        else
        {
            if (typeof(TResponse).IsDerivedFromOneOfType<InvalidResult>())
            { 
                var invalidResult = new InvalidResult(failures);
                return invalidResult.MapToOneOf<TResponse>();
            }
        }

        throw new ValidationException(failures);
    }
}
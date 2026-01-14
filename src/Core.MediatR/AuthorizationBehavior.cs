using MediatR;

namespace CraftersCloud.Core.MediatR;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
{
    private readonly IEnumerable<IAuthorizationRule<TRequest>> _authorizationRules;

    public AuthorizationBehavior(IEnumerable<IAuthorizationRule<TRequest>> authorizationRules)
    {
        _authorizationRules = authorizationRules;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await ApplyAuthorizationRulesAsync(request, cancellationToken);
        return await next();
    }

    private Task ApplyAuthorizationRulesAsync(TRequest request, CancellationToken cancellationToken)
    {
        return !_authorizationRules.Any() ? Task.CompletedTask : Task.WhenAll(_authorizationRules.Select(rule => rule.ExecuteAsync(request, cancellationToken)));
    }
}
namespace CraftersCloud.Core.MediatR;

public interface IAuthorizationRule<in TRequest>
{
    Task ExecuteAsync(TRequest request, CancellationToken token = default);
}
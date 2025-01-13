using MediatR;

namespace CraftersCloud.Core.Cqrs;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>;
using MediatR;

namespace CraftersCloud.Core.Messaging;

[PublicAPI]
public interface IBaseCommand
{
    /// <summary>
    /// Transaction behavior of the command. 
    /// </summary>
    public CommandTransactionBehavior TransactionBehavior => CommandTransactionBehavior.Default;
}

/// <summary>
/// Marks a class as a command.
/// </summary>
[PublicAPI]
public interface ICommand : IBaseCommand, IRequest;

/// <summary>
/// Marks a class as a command that returns a response.
/// </summary>
/// <typeparam name="TResponse">Response</typeparam>
[PublicAPI]
public interface ICommand<out TResponse> : IBaseCommand, IRequest<TResponse>;
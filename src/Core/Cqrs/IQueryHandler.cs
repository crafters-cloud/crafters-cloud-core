using MediatR;

namespace CraftersCloud.Core.Cqrs;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>;
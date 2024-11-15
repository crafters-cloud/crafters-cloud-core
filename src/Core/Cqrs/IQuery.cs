using MediatR;

namespace CraftersCloud.Core.Cqrs;

/// <summary>
/// Base interface for all the queries.
/// </summary>
public interface IBaseQuery;

/// <summary>
/// Baser query for all the queries that return a response.
/// </summary>
/// <typeparam name="TResponse">Type of response</typeparam>
public interface IQuery<out TResponse> : IBaseQuery, IRequest<TResponse>;
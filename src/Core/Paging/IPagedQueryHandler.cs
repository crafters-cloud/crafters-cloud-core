using CraftersCloud.Core.Messaging;

namespace CraftersCloud.Core.Paging;

public interface IPagedQueryHandler<in TRequest, TResponse> : IQueryHandler<TRequest, PagedQueryResponse<TResponse>>
    where TRequest : PagedQuery<TResponse>
{
}
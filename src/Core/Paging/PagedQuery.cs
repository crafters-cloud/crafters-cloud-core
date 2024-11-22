using CraftersCloud.Core.Messaging;

namespace CraftersCloud.Core.Paging;

public class PagedQuery<TResponse> : IQuery<PagedQueryResponse<TResponse>>, IPagedQuery
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}
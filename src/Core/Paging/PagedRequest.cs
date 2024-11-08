using MediatR;

namespace CraftersCloud.Core.Paging;

public class PagedRequest<TResponse> : IRequest<PagedResponse<TResponse>>, IPagedRequest
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}
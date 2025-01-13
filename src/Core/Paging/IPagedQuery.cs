using CraftersCloud.Core.Cqrs;

namespace CraftersCloud.Core.Paging;

public interface IPagedQuery : IBaseQuery
{
    int? PageNumber { get; set; }
    int? PageSize { get; set; }
    string? SortBy { get; set; }
    string? SortDirection { get; set; }
}
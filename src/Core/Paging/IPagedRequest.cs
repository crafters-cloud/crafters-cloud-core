using CraftersCloud.Core.Cqrs;

namespace CraftersCloud.Core.Paging;

public interface IPagedRequest : IBaseQuery
{
    int PageNumber { get; set; }
    int PageSize { get; set; }
    string SortBy { get; set; }
    string SortDirection { get; set; }
}
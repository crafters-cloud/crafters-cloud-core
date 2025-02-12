﻿namespace CraftersCloud.Core.Paging;

[PublicAPI]
public class PagedQuery<TResponse> : IPagedQuery
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}
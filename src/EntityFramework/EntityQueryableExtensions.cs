using System.Linq.Dynamic.Core;
using CraftersCloud.Core.Entities;
using CraftersCloud.Core.Paging;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.EntityFramework;

public static class EntityQueryableExtensions
{
    public static PagedQueryResponse<T> ToPagedResponse<T>(this IQueryable<T> query, IPagedQuery request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var pagedQuery = query
            .OrderByDynamic(request.SortBy, request.SortDirection ?? string.Empty);

        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? 10;
        var skipPaging = pageSize == int.MaxValue;
        if (!skipPaging)
        {
            pagedQuery = pagedQuery.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        var items = pagedQuery.ToList();

        var totalCount = skipPaging ? items.Count : query.Count();

        return new PagedQueryResponse<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = pageNumber,
            PageNumber = pageSize
        };
    }

    public static async Task<PagedQueryResponse<T>> ToPagedResponseAsync<T>(this IQueryable<T> query, IPagedQuery request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var pagedQuery = query
            .OrderByDynamic(request.SortBy, request.SortDirection ?? string.Empty);

        var pageNumber = request.PageNumber ?? 1;
        var pageSize = request.PageSize ?? 10;
        var skipPaging = pageSize == int.MaxValue;

        if (!skipPaging)
        {
            pagedQuery = pagedQuery.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        var items = await pagedQuery.ToListAsync(cancellationToken);

        var totalCount = skipPaging ? items.Count : await query.CountAsync(cancellationToken);

        return new PagedQueryResponse<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = pageSize,
            PageNumber = pageNumber
        };
    }
    
    private static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string? orderBy,
        string orderDirection = "asc") =>
        string.IsNullOrWhiteSpace(orderBy) ? query : query.OrderBy($"{orderBy} {orderDirection}");
}
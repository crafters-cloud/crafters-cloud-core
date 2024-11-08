﻿using System.Linq.Dynamic.Core;
using CraftersCloud.Core.Entities;
using CraftersCloud.Core.Paging;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.EntityFramework;

public static class EntityQueryableExtensions
{
    public static async Task<T> SingleOrNotFoundAsync<T>(this IQueryable<T> query,
        CancellationToken cancellationToken = default)
    {
        var result = await query.SingleOrDefaultAsync(cancellationToken);
        return result ?? throw new EntityNotFoundException(typeof(T).Name);
    }
    

    public static PagedResponse<T> ToPagedResponse<T>(this IQueryable<T> query, IPagedRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var pagedQuery = query
            .OrderByDynamic(request.SortBy, request.SortDirection);

        var skipPaging = request.PageSize == int.MaxValue;

        if (!skipPaging)
        {
            pagedQuery = pagedQuery.Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);
        }

        var items = pagedQuery.ToList();

        var totalCount = skipPaging ? items.Count : query.Count();

        return new PagedResponse<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = request.PageSize,
            PageNumber = request.PageNumber
        };
    }

    public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(this IQueryable<T> query, IPagedRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var pagedQuery = query
            .OrderByDynamic(request.SortBy, request.SortDirection);

        var skipPaging = request.PageSize == int.MaxValue;

        if (!skipPaging)
        {
            pagedQuery = pagedQuery.Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);
        }

        var items = await pagedQuery.ToListAsync(cancellationToken);

        var totalCount = skipPaging ? items.Count : await query.CountAsync(cancellationToken);

        return new PagedResponse<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageSize = request.PageSize,
            PageNumber = request.PageNumber
        };
    }

    private static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string? orderBy,
        string orderDirection = "asc") =>
        string.IsNullOrWhiteSpace(orderBy) ? query : query.OrderBy($"{orderBy} {orderDirection}");
}
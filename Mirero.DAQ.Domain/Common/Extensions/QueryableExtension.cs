 using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Dtos;
using Mirero.DAQ.Domain.Common.Protos;

namespace Mirero.DAQ.Domain.Common.Extensions;

public static class QueryableExtension
{
    public static (int Count, IEnumerable<T> Items) AsPagedResult<T>(this IQueryable<T> queryable,
        QueryParameter? queryParameter)
    {
        var parameter = queryParameter ?? new QueryParameter();

        var pageIndex = Math.Max(0, parameter.PageIndex);
        var pageSize = parameter.PageSize;
        var where = (string.IsNullOrEmpty(parameter.Where)) ? "true" : parameter.Where;
        var orderBy = (string.IsNullOrEmpty(parameter.OrderBy)) ? "true" : parameter.OrderBy;

        var count = queryable.Count();

        var items = queryable.Where(where).OrderBy(orderBy) as IQueryable<T>;
        if(pageSize != 0) 
        {
            items = items
                .Skip(pageSize * pageIndex)
                .Take(pageSize);
        }

        return (count, items.ToList());
    }

    public static async Task<(int Count, IEnumerable<T> Items)> AsPagedResultAsync<T>(
        this IQueryable<T> queryable,
        QueryParameter queryParameter,
        CancellationToken cancellationToken = default)
    {
        var parameter = queryParameter ?? new QueryParameter();

        var pageIndex = Math.Max(0, parameter.PageIndex);
        var pageSize = Math.Max(1, parameter.PageSize);
        var where = (string.IsNullOrEmpty(parameter.Where)) ? "true" : parameter.Where;
        var orderBy = (string.IsNullOrEmpty(parameter.OrderBy)) ? "true" : parameter.OrderBy;

        var count = await queryable.CountAsync(cancellationToken: cancellationToken);

        var items = await queryable
            .Where(where)
            .OrderBy(orderBy)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        return (count, items);
    }

    public static async Task<(int Count, IEnumerable<TModel> Items)> AsPagedResultAsync<T, TModel>(
        this IQueryable<T> queryable,
        QueryParameter queryParameter,
        Func<T, TModel> func,
        CancellationToken cancellationToken = default)
    {
        var (count, items) = await queryable.AsPagedResultAsync(queryParameter, cancellationToken);
        return (count, items.Select(func));
    }

    public static IQueryable<T> AsPagedQueryableResult<T>(
        this IQueryable<T> queryable,
        QueryParameter? queryParameter)
    {
        var parameter = queryParameter ?? new QueryParameter();

        var pageIndex = Math.Max(0, parameter.PageIndex);
        var pageSize = Math.Max(1, parameter.PageSize);
        var where = (string.IsNullOrEmpty(parameter.Where)) ? "true" : parameter.Where;
        var orderBy = (string.IsNullOrEmpty(parameter.OrderBy)) ? "true" : parameter.OrderBy;

        var items = queryable
            .Where(where)
            .OrderBy(orderBy)
            .Skip(pageSize * pageIndex)
            .Take(pageSize);

        return items;
    }
}
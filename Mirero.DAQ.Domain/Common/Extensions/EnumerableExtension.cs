using Mirero.DAQ.Domain.Common.Protos;

namespace Mirero.DAQ.Domain.Common.Extensions;

public static class EnumerableExtension
{
    /*public static (int Count, IEnumerable<T> Items) AsPagedResult<T>(this IEnumerable<T> enumerable,
        QueryParameter? queryParameter)
    {
        var parameter = queryParameter ?? new QueryParameter();
    
        var pageIndex = Math.Max(0, parameter.PageIndex);
        var pageSize = parameter.PageSize;
        var where = (string.IsNullOrEmpty(parameter.Where)) ? "true" : parameter.Where;
        var orderBy = (string.IsNullOrEmpty(parameter.OrderBy)) ? "true" : parameter.OrderBy;
    
        var count = enumerable.Count();
        
        
    
        var items = enumerable.Where(where).OrderBy(orderBy) as IQueryable<T>;
        if(pageSize != 0) 
        {
            items = items
                .Skip(pageSize * pageIndex)
                .Take(pageSize);
        }
    
        return (count, items.ToList());
    }*/
}
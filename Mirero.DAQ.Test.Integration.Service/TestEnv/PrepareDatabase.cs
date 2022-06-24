using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mirero.DAQ.Test.Integration.Service;

public static class ServiceProviderExtensions
{
    public static bool InitializeDb<T>(this IServiceProvider services, string sqlFileName)
        where T : DbContext
    {
        var sql = EmbeddedResourceUtils.ReadFromResourceFile(sqlFileName);
        if(!string.IsNullOrEmpty(sql))
        {
            try
            {
                using var dbContext = services.GetRequiredService<T>();
                var database = dbContext.Database;
                database.BeginTransaction();
                database.ExecuteSqlRaw(sql);
                database.CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        return false;
    }
    
    public static bool InitializeDb<T>(this IServiceProvider services, List<string> sqlFileNames)
        where T : DbContext
    {
        var sqls = sqlFileNames
            .Select(x => EmbeddedResourceUtils.ReadFromResourceFile(x))
            .Where(x => !string.IsNullOrEmpty(x)).ToList();
        
        if(sqls.Count != 0)
        {
            try
            {
                using var dbContext = services.GetRequiredService<T>();
                var database = dbContext.Database;
                database.BeginTransaction();
                foreach (var sql in sqls)
                {
                    database.ExecuteSqlRaw(sql);
                }
                database.CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        return false;
    }
}
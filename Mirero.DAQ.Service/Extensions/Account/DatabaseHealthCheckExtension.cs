using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mirero.DAQ.Service.Extensions.Account;

public static class DatabaseHealthCheckExtension
{
    public static IServiceCollection AddAccountPostgreSQLHealthCheck(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks().AddNpgSql(
            configuration.GetConnectionString("AccountDb"),
            name: "account-db-check",
            tags: new string[] {"account"});
        
        return services;
    }
}
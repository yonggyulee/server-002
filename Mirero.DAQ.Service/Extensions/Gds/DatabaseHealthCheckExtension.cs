using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mirero.DAQ.Service.Extensions.Gds;

public static class DatabaseHealthCheckExtension
{
    public static IServiceCollection AddGdsPostgreSQLHealthCheck(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks().AddNpgSql(
            configuration.GetConnectionString("GdsDb"),
            name: "gds-db-check",
            tags: new string[] {"gds"});
        
        return services;
    }
}
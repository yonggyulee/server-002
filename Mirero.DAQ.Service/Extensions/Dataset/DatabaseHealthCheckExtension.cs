using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mirero.DAQ.Service.Extensions.Dataset;

public static class DatabaseHealthCheckExtension
{
    public static IServiceCollection AddDatasetPostgreSQLHealthCheck(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks().AddNpgSql(
            configuration.GetConnectionString("DatasetDb"),
            name: "dataset-db-check",
            tags: new string[] {"dataset"});

        return services;
    }
}
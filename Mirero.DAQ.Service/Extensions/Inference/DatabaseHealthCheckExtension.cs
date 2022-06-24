using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mirero.DAQ.Service.Extensions.Inference;

public static class DatabaseHealthCheckExtension
{
    public static IServiceCollection AddInferencePostgreSQLHealthCheck(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks().AddNpgSql(
            configuration.GetConnectionString("InferenceDb"),
            name: "inference-db-check",
            tags: new string[] {"inference"});
        
        return services;
    }
}
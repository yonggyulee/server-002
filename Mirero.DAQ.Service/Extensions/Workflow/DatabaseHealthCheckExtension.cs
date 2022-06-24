using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mirero.DAQ.Service.Extensions.Workflow;

public static class DatabaseHealthCheckExtension
{
    public static IServiceCollection AddWorkflowPostgreSQLHealthCheck(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks().AddNpgSql(
            configuration.GetConnectionString("WorkflowDb"),
            name: "workflow-db-check",
            tags: new string[] {"workflow"});

        return services;
    }
}
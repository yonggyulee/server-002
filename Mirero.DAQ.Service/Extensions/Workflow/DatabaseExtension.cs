using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Service.Extensions.Workflow;

public static class DatabaseExtension
{
    public static IServiceCollection AddWorkflowPostgreSQLDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var service = configuration.GetValue("service", "Api.Offline");
        
        services.AddDbContextFactory<WorkflowDbContextPostgreSQL>(options => _ = service switch
        {
            "Api.Offline" => options.UseNpgsql(configuration.GetConnectionString("WorkflowDb"),
                    b => b.MigrationsAssembly("Mirero.DAQ.Service.Api.Offline"))
                .UseSnakeCaseNamingConvention()
                .EnableSensitiveDataLogging(),
            _ => throw new Exception($"Unsupported service : {service}")
        });

        return services;
    }
}
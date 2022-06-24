using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Infrastructure.Database.Inference;

namespace Mirero.DAQ.Service.Extensions.Inference;

public static class DatabaseExtension
{
    public static IServiceCollection AddInferencePostgreSQLDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var service = configuration.GetValue("service", "Api.Offline");
        
        services.AddDbContextFactory<InferenceDbContextPostgreSQL>(options => _ = service switch
        {
            "Api.Offline" => options.UseNpgsql(configuration.GetConnectionString("InferenceDb"),
                    b => b.MigrationsAssembly("Mirero.DAQ.Service.Api.Offline"))
                .UseSnakeCaseNamingConvention()
                .EnableSensitiveDataLogging(),
            _ => throw new Exception($"Unsupported service : {service}")
        });
        
        return services;
    }
}
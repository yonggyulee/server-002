using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Infrastructure.Database.Dataset;

namespace Mirero.DAQ.Service.Extensions.Dataset;

public static class DatabaseExtension
{
    public static IServiceCollection AddDatasetPostgreSQLDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var service = configuration.GetValue("service", "Api.Offline");
        
        services.AddDbContextFactory<DatasetDbContextPostgreSQL>(options => _ = service switch
        {
            "Api.Offline" => options.UseNpgsql(configuration.GetConnectionString("DatasetDb"),
                    b => b.MigrationsAssembly("Mirero.DAQ.Service.Api.Offline"))
                .UseSnakeCaseNamingConvention()
                .EnableSensitiveDataLogging(),
            _ => throw new Exception($"Unsupported service : {service}")
        });
        
        return services;
    }
}
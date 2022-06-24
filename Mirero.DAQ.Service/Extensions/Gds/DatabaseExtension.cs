using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Infrastructure.Database.Gds;

namespace Mirero.DAQ.Service.Extensions.Gds;

public static class DatabaseExtension
{
    public static IServiceCollection AddGdsPostgreSQLDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var service = configuration.GetValue("service", "Api.Offline");
        
        services.AddDbContextFactory<GdsDbContextPostgreSQL>(options => _ = service switch
        {
            "Api.Offline" => options.UseNpgsql(configuration.GetConnectionString("GdsDb"),
                    b => b.MigrationsAssembly("Mirero.DAQ.Service.Api.Offline"))
                .UseSnakeCaseNamingConvention()
                .EnableSensitiveDataLogging(),
            _ => throw new Exception($"Unsupported service : {service}")
        });

        return services;
    }
}
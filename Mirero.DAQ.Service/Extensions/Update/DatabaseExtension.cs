using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Database.Update;

namespace Mirero.DAQ.Service.Extensions.Update;

public static class DatabaseExtension
{
    public static IServiceCollection AddUpdateSqliteDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var service = configuration.GetValue("service", "Api.Update");

        services.AddDbContextFactory<UpdateDbContextInmemory>(options => _ = service switch
        {
            "Api.Update" => options.UseInMemoryDatabase(databaseName: "update")
                .UseSnakeCaseNamingConvention()
                .EnableSensitiveDataLogging(),
            _ => throw new Exception($"Unsupported service : {service}")
        });

        return services;
    }
}
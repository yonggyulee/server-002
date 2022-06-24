using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Infrastructure.Database.Account;

namespace Mirero.DAQ.Service.Extensions.Account;

public static class DatabaseExtension
{
    public static IServiceCollection AddAccountPostgreSQLDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var service = configuration.GetValue("service", "Api.Account");
        
        services.AddDbContextFactory<AccountDbContextPostgreSQL>(options => _ = service switch
        {
            "Api.Account" => options.UseNpgsql(configuration.GetConnectionString("AccountDb"),
                    b => b.MigrationsAssembly("Mirero.DAQ.Service.Api.Account"))
                .UseSnakeCaseNamingConvention()
                .EnableSensitiveDataLogging(),
            _ => throw new Exception($"Unsupported service : {service}")
        });
        
        return services;
    }
}
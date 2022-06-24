using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Service.Extensions.Account;

namespace Mirero.DAQ.Service.Extensions;

public static class AccountExtension
{
    public static IServiceCollection AddAccountService(this IServiceCollection service,
        IConfiguration configuration)
    {
        service.AddAccountIntegrations(configuration);
        service.AddAccountPostgreSQLDatabase(configuration);
        service.AddAccountPostgreSQLHealthCheck(configuration);
        service.AddAccountValidator(configuration);
        
        return service;
    }
}
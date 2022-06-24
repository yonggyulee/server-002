using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Infrastructure.Identity;

namespace Mirero.DAQ.Service.Extensions.Account;

public static class IntegrationExtension
{
    public static IServiceCollection AddAccountIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(Mirero.DAQ.Application.Account.Handlers.AccountHandlerBase));
        
        services.AddTransient<ITokenManager, JwtTokenManager>();

        return services;
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Container;
using Mirero.DAQ.Service.Interceptors.Validations;
using Mirero.DAQ.Service.Middlewares.Authorization;

namespace Mirero.DAQ.Service.Extensions.Common;

public static class IntegrationExtension
{
    public static IServiceCollection AddCommonIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<RequesterContext>();
        services.AddSingleton<IValidatorErrorMessageHandler, DefaultErrorMessageHandler>();
        services.AddScoped<IValidatorLocator>(p => new ServiceCollectionValidationProvider(p));
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, MyAuthorizationMiddlewareResultHandler>();

        return services;
    }
}
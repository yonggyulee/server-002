using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using Mirero.DAQ.Application.Gds.UriGenerator;

namespace Mirero.DAQ.Service.Extensions.Gds;

public static class IntegrationExtension
{
    public static IServiceCollection AddGdsIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(Mirero.DAQ.Application.Gds.Handlers.GdsHandlerBase));

        services.AddTransient<IFileStorage, FolderFileStorage>();
        services.AddTransient<IPostgresLockProviderFactory, AdvisoryLockProviderFactory>();
        services.AddGdsValidator(configuration);
        services.AddTransient<IUriGenerator, FolderUriGenerator>();

        return services;
    }
}
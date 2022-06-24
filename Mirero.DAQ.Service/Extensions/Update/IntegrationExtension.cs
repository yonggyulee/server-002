using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Application.Update.UriGenerator;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Service.Extensions.Update;

public static class IntegrationExtension
{
    public static IServiceCollection AddUpdateIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(Mirero.DAQ.Application.Update.Handlers.UpdateHandlerBase));

        services.AddTransient<IFileStorage, FolderFileStorage>();
        services.AddTransient<IUriGenerator, FolderUriGenerator>();
        
        return services;
    }
}
using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Service.Extensions.Dataset;

public static class IntegrationExtension
{
    public static IServiceCollection AddDatasetIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(Mirero.DAQ.Application.Dataset.Handlers.DatasetHandlerBase));

        services.AddTransient<IFileStorage, FolderFileStorage>();
        services.AddTransient<IPostgresLockProviderFactory, AdvisoryLockProviderFactory>();

        return services;
    }
}
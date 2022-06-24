using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Application.Inference.Option;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Infrastructure.Caching;
using Mirero.DAQ.Infrastructure.Grpc.Client;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Service.Extensions.Inference;

public static class IntegrationExtension
{
    public static IServiceCollection AddInferenceIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(Mirero.DAQ.Application.Inference.Handlers.InferenceHandlerBase));

        services.AddSingleton<ICacheItemProvider<string, GrpcChannelData>, GrpcChannelProvider>();

        services.AddTransient<IFileStorage, FolderFileStorage>();
        services.AddTransient<IUriGenerator, FolderUriGenerator>();
        services.AddTransient<IPostgresLockProviderFactory, AdvisoryLockProviderFactory>();
        
        services.AddScoped(_ => configuration.GetSection("Inference:WorkerRetryOptions").Get<WorkerRetryOption>());

        return services;
    }
}
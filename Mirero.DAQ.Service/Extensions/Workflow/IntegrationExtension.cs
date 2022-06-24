using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Application.Workflow.ListJobsManager;
using Mirero.DAQ.Application.Workflow.StreamCreator;
using Mirero.DAQ.Application.Workflow.UriGenerator;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Service.Extensions.Workflow;

public static class IntegrationExtension
{
    public static IServiceCollection AddWorkflowIntegrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(typeof(Mirero.DAQ.Application.Workflow.Handlers.WorkflowHandlerBase));
        services.AddTransient<IFileStorage, FolderFileStorage>();
        services.AddTransient<IUriGenerator, FolderUriGenerator>();
        services.AddTransient<IPostgresLockProviderFactory, AdvisoryLockProviderFactory>();
        services.AddTransient<WorkflowVersionUploadStreamCreator>();
        services.AddTransient<WorkflowDownloadStreamCreator>();     
        services.AddSingleton<Infrastructure.Redis.Connection>();
        services.AddTransient<ListJobManagerFactory>()
            .AddTransient<PostgreSqListJobsManager>()
            .AddTransient<RedisListJobsManager>();
        services.AddSingleton(x => new Infrastructure.Redis.ConnectionConfig
        {
            Uris = new Uri[]{ new(configuration.GetConnectionString("JobManager")) } 
        });

        return services;
    }
}
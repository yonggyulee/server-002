using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mirero.DAQ.Service.Extensions.Workflow;

public static class BackgroundServiceExtension
{
    public static IServiceCollection AddBackgroundService(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddHostedService<RecipeStatusUpdateWorker>();

        return services;
    }
}
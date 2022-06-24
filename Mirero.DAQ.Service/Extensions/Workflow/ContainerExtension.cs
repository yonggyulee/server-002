using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mirero.DAQ.Service.Extensions.Workflow;

public static class ContainerExtension
{
    public static IServiceCollection AddDockerContainer(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
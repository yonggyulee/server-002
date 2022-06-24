using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Service.Extensions.Inference;

namespace Mirero.DAQ.Service.Extensions;

public static class InferenceExtension
{
    public static IServiceCollection AddInferenceService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddInferenceIntegrations(configuration);
        service.AddInferencePostgreSQLDatabase(configuration);
        service.AddInferencePostgreSQLHealthCheck(configuration);
        
        return service;
    }
}
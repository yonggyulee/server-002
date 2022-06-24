using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Service.Extensions.Dataset;

namespace Mirero.DAQ.Service.Extensions;

public static class DatasetExtension
{
    public static IServiceCollection AddDatasetService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDatasetIntegrations(configuration);
        service.AddDatasetPostgreSQLDatabase(configuration);
        service.AddDatasetPostgreSQLHealthCheck(configuration);
        service.AddDatasetValidator(configuration);
        
        return service;
    }
}
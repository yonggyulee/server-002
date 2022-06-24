using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Mirero.DAQ.Service.Extensions.Gds;

namespace Mirero.DAQ.Service.Extensions;

public static class GdsExtension
{
    public static IServiceCollection AddGdsService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddGdsIntegrations(configuration);
        service.AddGdsPostgreSQLDatabase(configuration);
        service.AddGdsPostgreSQLHealthCheck(configuration);
        
        return service;
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Service.Extensions.Update;

namespace Mirero.DAQ.Service.Extensions;

public static class UpdateExtenstion
{
    public static IServiceCollection AddUpdateService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddUpdateSqliteDatabase(configuration);
        service.AddUpdateIntegrations(configuration);
        
        return service;
    }
}
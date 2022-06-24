using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Service.Extensions.Workflow;

namespace Mirero.DAQ.Service.Extensions;

public static class WorkflowExtension
{
    public static IServiceCollection AddWorkflowService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddWorkflowIntegrations(configuration);
        service.AddWorkflowPostgreSQLDatabase(configuration);
        service.AddWorkflowPostgreSQLHealthCheck(configuration);
        
        return service;
    }
    
}
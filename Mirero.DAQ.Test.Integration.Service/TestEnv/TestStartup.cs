using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mirero.DAQ.Service.Extensions.Account;
using Mirero.DAQ.Service.Extensions.Common;
using Mirero.DAQ.Service.Extensions.Dataset;
using Mirero.DAQ.Service.Extensions.Gds;
using Mirero.DAQ.Service.Extensions.Inference;
using Mirero.DAQ.Service.Extensions.Update;
using Mirero.DAQ.Service.Extensions.Workflow;
using Mirero.DAQ.Service.Interceptors;
using Mirero.DAQ.Service.Services.Dataset;
using Mirero.DAQ.Service.Services.Account;
using Mirero.DAQ.Service.Services.Update;
using Mirero.DAQ.Service.Services.Workflow;
using Mirero.DAQ.Service.Services.Gds;
using Mirero.DAQ.Service.Services.Inference;
using GrpcServerService = Mirero.DAQ.Service.Services.Inference.GrpcServerService;
using GrpcVolumeService = Mirero.DAQ.Service.Services.Inference.GrpcVolumeService;
using GrpcWorkerService = Mirero.DAQ.Service.Services.Workflow.GrpcWorkerService;

public class TestStartup
{
    private readonly IConfiguration configuration;

    public TestStartup(IConfiguration configuration)
    {
        this.configuration = configuration;

        var cs = configuration.GetSection("ConnectionString");
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IMapper, ServiceMapper>();
        var config = new TypeAdapterConfig();
        config.AddCommonMapperConfig();
        config.AddAccountMapperConfig();
        config.AddDatasetMapperConfig();
        config.AddWorkflowMapperConfig();
        config.AddGdsServiceMapperConfig();
        config.AddUpdateMapperConfig(); //update di
        config.Compile();
        services.AddSingleton(config);

        services.AddAccountPostgreSQLDatabase(configuration);
        services.AddAccountPostgreSQLHealthCheck(configuration);
        services.AddAccountValidator(configuration);
        services.AddAccountIntegrations(configuration);

        services.AddDatasetPostgreSQLDatabase(configuration);
        services.AddDatasetPostgreSQLHealthCheck(configuration);
        services.AddDatasetValidator(configuration);
        services.AddDatasetIntegrations(configuration);

        services.AddInferencePostgreSQLDatabase(configuration);
        services.AddInferencePostgreSQLHealthCheck(configuration);
        services.AddInferenceIntegrations(configuration);

        services.AddGdsPostgreSQLDatabase(configuration);
        services.AddGdsPostgreSQLHealthCheck(configuration);
        services.AddGdsIntegrations(configuration);

        services.AddUpdateSqliteDatabase(configuration);
        services.AddUpdateIntegrations(configuration);
        
        services.AddWorkflowIntegrations(configuration);
        services.AddWorkflowPostgreSQLDatabase(configuration);
        services.AddWorkflowPostgreSQLHealthCheck(configuration);

        services.AddCommonIntegrations(configuration);
        services.AddAuthentication(configuration);
        services.AddAuthorization(options =>
        {
            options.AddAccountAuthorizationPolicy();
            options.AddDatasetAuthorizationPolicy();
            options.AddGdsAuthorizationPolicy();
            options.AddInferenceAuthorizationPolicy();
            options.AddWorkflowAuthorizationPolicy();
        });

        services.AddGrpc(options =>
        {
            options.Interceptors.Add<RequesterContextInterceptor>();
            options.Interceptors.Add<LoggingInterceptor>();
            options.Interceptors.Add<ValidationInterceptor>();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(x =>
        {
            _UseDatasetService(x);
            _UseInferenceService(x);
            _UseGdsService(x);
            _UseUpdateService(x);
            
            x.MapGrpcService<GrpcUserService>();
            x.MapGrpcService<GrpcGroupService>();
            x.MapGrpcService<GrpcSignInService>();
            
            x.MapGrpcService<Mirero.DAQ.Service.Services.Workflow.GrpcVolumeService>();
            x.MapGrpcService<Mirero.DAQ.Service.Services.Workflow.GrpcServerService>();
            x.MapGrpcService<GrpcWorkerService>();
            x.MapGrpcService<GrpcWorkflowService>();
            x.MapGrpcService<GrpcJobService>();
        });
    }

    private void _UseUpdateService(IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<GrpcMppUpdateService>();
        builder.MapGrpcService<GrpcRcUpdateService>();
    }
    
    private void _UseDatasetService(IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<GrpcClassCodeService>();
        builder.MapGrpcService<GrpcGtDatasetService>();
        builder.MapGrpcService<GrpcImageDatasetService>();
        builder.MapGrpcService<GrpcTrainingDataService>();
    }

    private void _UseInferenceService(IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<GrpcInferenceService>();
        builder.MapGrpcService<GrpcModelService>();
        builder.MapGrpcService<GrpcVolumeService>();
        builder.MapGrpcService<GrpcServerService>();
        builder.MapGrpcService<GrpcWorkerService>();
    }

    private void _UseGdsService(IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<Mirero.DAQ.Service.Services.Gds.GrpcVolumeService>();
        builder.MapGrpcService<Mirero.DAQ.Service.Services.Gds.GrpcServerService>();
        builder.MapGrpcService<Mirero.DAQ.Service.Services.Gds.GrpcGdsService>();
    }
}
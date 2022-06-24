using System.Net;
using Mapster;
using MapsterMapper;
using Mirero.DAQ.Service.Extensions;
using Mirero.DAQ.Service.Extensions.Common;
using Mirero.DAQ.Service.Extensions.Dataset;
using Mirero.DAQ.Service.Extensions.Gds;
using Mirero.DAQ.Service.Extensions.Inference;
using Mirero.DAQ.Service.Extensions.Workflow;
using Mirero.DAQ.Service.Interceptors;
using Serilog;
using System.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Core;

try
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
        .AddJsonFile("appsettings.common.json", false, true)
        .AddJsonFile("appsettings.offline.json", false, true)
        .AddJsonFile("appsettings.inference.json", false, true)
        .AddEnvironmentVariables()
        .Build();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration).CreateLogger();

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    // builder.WebHost.AddKestrelServer(configuration);
    builder.WebHost.ConfigureKestrel(options =>
    {
        // var urlString = configuration.GetValue<string>("Kestrel:Endpoints:Grpc:Url");
        // if (!Uri.TryCreate(urlString, UriKind.Absolute, out var uri))
        // {
        //     
        // }
        options.ListenAnyIP(5020, listenOptions =>  listenOptions.Protocols = HttpProtocols.Http2);
        options.ListenAnyIP(5021, listenOptions =>  listenOptions.Protocols = HttpProtocols.Http1);
    });

    
    builder.Configuration.AddConfiguration(configuration);
    builder.Services.AddControllers();

    builder.Services.AddScoped<IMapper, ServiceMapper>();
    var config = new TypeAdapterConfig();
    config.AddCommonMapperConfig();
    config.AddDatasetMapperConfig();
    config.AddGdsServiceMapperConfig();
    config.AddInferenceMapperConfig();
    config.AddWorkflowMapperConfig();
    config.Compile();
    builder.Services.AddSingleton(config);

    builder.Services.AddCommonIntegrations(configuration);
    builder.Services.AddDatasetService(configuration);
    builder.Services.AddGdsService(configuration);
    builder.Services.AddInferenceService(configuration);
    builder.Services.AddWorkflowService(configuration);

    builder.Services.AddAuthentication(configuration);
    builder.Services.AddAuthorization(options =>
    {
        options.AddDatasetAuthorizationPolicy();
        options.AddGdsAuthorizationPolicy();
        options.AddInferenceAuthorizationPolicy();
        options.AddWorkflowAuthorizationPolicy();
    });

    builder.Services.AddGrpc(options =>
    {
        options.Interceptors.Add<RequesterContextInterceptor>();
        options.Interceptors.Add<LoggingInterceptor>();
        options.Interceptors.Add<ValidationInterceptor>();
    });

    var app = builder.Build();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseDatasetService();
    app.UseInferenceService();
    app.UseGdsService();
    app.UseWorkflowService();
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
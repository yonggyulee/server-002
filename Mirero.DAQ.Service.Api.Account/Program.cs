using Mapster;
using MapsterMapper;
using Mirero.DAQ.Service.Extensions;
using Mirero.DAQ.Service.Extensions.Account;
using Mirero.DAQ.Service.Extensions.Common;
using Mirero.DAQ.Service.Interceptors;
using Serilog;
using System.Reflection;
using Mirero.DAQ.Service.Services.Account;

try
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
        .AddJsonFile("appsettings.common.json", false, true)
        .AddJsonFile("appsettings.account.json", false, true)
        .AddEnvironmentVariables()
        .Build();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration).CreateLogger();

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    builder.WebHost.AddKestrelServer(configuration);
    builder.Configuration.AddConfiguration(configuration);

    builder.Services.AddScoped<IMapper, ServiceMapper>();
    var config = new TypeAdapterConfig();
    config.AddCommonMapperConfig();
    config.AddAccountMapperConfig();
    config.Compile();
    builder.Services.AddSingleton(config);
    
    builder.Services.AddAccountService(configuration);
    builder.Services.AddCommonIntegrations(configuration);
    builder.Services.AddAuthentication(configuration);
    builder.Services.AddAuthorization(options =>
    {
        options.AddAccountAuthorizationPolicy();
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
    app.MapGrpcService<GrpcUserService>();
    app.MapGrpcService<GrpcGroupService>();
    app.MapGrpcService<GrpcSignInService>();
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
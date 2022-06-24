using System.Reflection;
using Mapster;
using MapsterMapper;
using Mirero.DAQ.Service.Extensions;
using Mirero.DAQ.Service.Extensions.Common;
using Mirero.DAQ.Service.Extensions.Update;
using Mirero.DAQ.Service.Interceptors;
using Serilog;

try
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
        .AddJsonFile("appsettings.common.json", false, true)
        .AddJsonFile("appsettings.update.json", false, true)
        .AddEnvironmentVariables()
        .Build();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration).CreateLogger();

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    builder.WebHost.AddKestrelServer(configuration);
    builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
    builder.Configuration.AddConfiguration(configuration);

    builder.Services.AddScoped<IMapper, ServiceMapper>();
    var config = new TypeAdapterConfig();
    config.AddCommonMapperConfig();
    config.AddUpdateMapperConfig();
    config.Compile();
    builder.Services.AddSingleton(config);
    builder.Services.AddCommonIntegrations(configuration);
    builder.Services.AddUpdateService(configuration);

    builder.Services.AddGrpc(options =>
    {
        options.Interceptors.Add<RequesterContextInterceptor>();
        options.Interceptors.Add<LoggingInterceptor>();
        options.Interceptors.Add<ValidationInterceptor>();
    });
    
    var app = builder.Build();
    app.UseUpdateService(configuration);
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
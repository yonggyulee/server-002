using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using Docker.DotNet;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Databases;
using DotNet.Testcontainers.Containers.WaitStrategies;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Account;
using Mirero.DAQ.Infrastructure.Database.Dataset;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Database.Gds;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Serilog;

namespace Mirero.DAQ.Test.Integration.Service;

public delegate void LogMessage(LogLevel logLevel, string categoryName, EventId eventId, string message, Exception? exception);

public  class ApiServiceIntegrationTestFixture<TStartup> : IDisposable where TStartup : class
{
    private TestServer? _server;
    private HttpMessageHandler? _handler;
    private GrpcChannel? _channel;
    
    public IHost? Host { get; private set; }
    public PostgreSqlTestcontainer? PgContainer { get; private set; }
    public event LogMessage? LoggedMessage;
    
    public GrpcChannel GrpcChannel => _channel ??= CreateChannel();

    protected GrpcChannel CreateChannel()
    {
        return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            LoggerFactory = LoggerFactory,
            HttpHandler = _handler
        });
    }

    private readonly PostgreSqlTestcontainer? test;

    
    public ApiServiceIntegrationTestFixture(Dictionary<string, string> configuration)
    {
        LoggerFactory = new LoggerFactory();
        LoggerFactory.AddProvider(new ForwardingLoggerProvider((logLevel, category, eventId, message, exception) =>
        {
            LoggedMessage?.Invoke(logLevel, category, eventId, message, exception);
        }));

        PgContainer = new TestcontainersBuilder<PostgreSqlTestcontainer?>()
            .WithDockerTestEnvironment()
            .WithDatabaseWithCustomPort(new PostgreSqlTestcontainerConfiguration
            {
                Database = "daq", Username = "mirero", Password = "system", Port = 65438
            })
            .WithImage("postgres:14.2")
            .WithUserNameSuffixed("pg-test-db")
            .Build();

        if (PgContainer is null)
        {
            throw new InvalidOperationException("Docker Container 생성 실패");
        }
        
        PgContainer.StartAsync().GetAwaiter().GetResult();

        configuration["ConnectionStrings:AccountDb"] = PgContainer.ConnectionString;
        configuration["ConnectionStrings:RecipeDb"] = PgContainer.ConnectionString;
        configuration["ConnectionStrings:InferenceDb"] = PgContainer.ConnectionString;
        configuration["ConnectionStrings:DatasetDb"] = PgContainer.ConnectionString;
        configuration["ConnectionStrings:GdsDb"] = PgContainer.ConnectionString;
        configuration["ConnectionStrings:WorkflowDb"] = PgContainer.ConnectionString;

        TestConfiguration = configuration;

        //Environment.SetEnvironmentVariable("ConnectionString__AccountDb","User ID=mirero;Password=system;Host=localhost;Port=5432;Database=daq");
       
        var builder = new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<ILoggerFactory>(LoggerFactory);
            })
            .ConfigureWebHostDefaults(webHost =>
            {
                webHost.ConfigureAppConfiguration(config =>
                {
                    config.Sources.Clear();
                    config.AddJsonFile("appsettings.common.json", false, true);
                    config.AddJsonFile("appsettings.account.json", false, true);
                    config.AddJsonFile("appsettings.update.json", false, true);
                    config.AddJsonFile("appsettings.offline.json", false, true);
                    config.AddInMemoryCollection(TestConfiguration);
                });
                
                webHost
                    .UseTestServer()
                    .UseStartup<TStartup>();

                webHost.UseSerilog();

                Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console()
                        .CreateLogger()
                    ;
            });
        Host = builder.Build();

        var services = Host.Services;
        services.InitializeDb<AccountDbContextPostgreSQL>("account-db.sql");
        services.InitializeDb<DatasetDbContextPostgreSQL>("dataset-db.sql");
        services.InitializeDb<InferenceDbContextPostgreSQL>("inference-db.sql");
        services.InitializeDb<WorkflowDbContextPostgreSQL>(new List<string>()
            { "workflow-db.sql"
                , "workflow-server-seed.sql"
                , "workflow-volume-seed.sql"
                , "workflow-worker-seed.sql"
                , "workflow-workflow-seed.sql"
                , "workflow-workflow-version-seed.sql"
                , "workflow-batch-job-seed.sql"
                , "workflow-job-seed.sql"
            });

        services.InitializeDb<GdsDbContextPostgreSQL>("gds-db.sql");

        Host.Start();
        _server = Host.GetTestServer();
        _handler = _server.CreateHandler();
    }

    public Dictionary<string, string> TestConfiguration { get; set; }

    public LoggerFactory LoggerFactory { get; }

    public HttpMessageHandler Handler
    {
        get
        {
            return _handler!;
        }
    }

    public virtual void Dispose()
    {
        PgContainer?.StopAsync().GetAwaiter().GetResult();
        PgContainer?.CleanUpAsync().GetAwaiter().GetResult();
        _handler?.Dispose();
        _server?.Dispose();
        Host?.StopAsync().GetAwaiter().GetResult();
        Host?.Dispose();

        PgContainer = null;
        _handler = null;
        _server = null;
        Host = null;
    }

    public IDisposable GetTestContext(TextWriter outputHelper)
    {
        return new GrpcTestContext<TStartup>(this, outputHelper);
    }
}

internal class GrpcTestContext<TStartup> : IDisposable where TStartup : class
{
    private readonly Stopwatch _stopwatch;
    private readonly ApiServiceIntegrationTestFixture<TStartup> _fixture;
    //private readonly ITestOutputHelper _outputHelper;
    private readonly TextWriter _outputHelper;

    public GrpcTestContext(ApiServiceIntegrationTestFixture<TStartup> fixture, TextWriter writer)
    {
        _stopwatch = Stopwatch.StartNew();
        _fixture = fixture;
        _outputHelper = writer;
        _fixture.LoggedMessage += WriteMessage;
    }

    private void WriteMessage(LogLevel logLevel, string category, EventId eventId, string message, Exception? exception)
    {
        _outputHelper.WriteLine($"{_stopwatch.Elapsed.TotalSeconds:N3}s {category} - {logLevel}: {message}");
    }

    public void Dispose()
    {
        _fixture.LoggedMessage -= WriteMessage;
    }
}
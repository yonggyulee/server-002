using System;
using System.IO;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

namespace Mirero.DAQ.Test.Integration.Service;

//public class IntegrationTestBase : IClassFixture<GrpcTestFixture<Startup>>, IDisposable
public class GrpcFixture<TStartup> : IDisposable 
    where TStartup : class
{
    private GrpcChannel? _channel;
    private readonly IDisposable? _testContext;

    public ApiServiceIntegrationTestFixture<TStartup> Fixture { get; set; }

    public ILoggerFactory LoggerFactory => Fixture.LoggerFactory;

    public GrpcChannel Channel => _channel ??= CreateChannel();

    protected GrpcChannel CreateChannel()
    {
        return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            LoggerFactory = LoggerFactory,
            HttpHandler = Fixture.Handler
        });
    }

    public GrpcFixture(ApiServiceIntegrationTestFixture<TStartup> fixture, TextWriter outputHelper)
    {
        Fixture = fixture;
        _testContext = Fixture.GetTestContext(outputHelper);
    }

    public void Dispose()
    {
        _testContext?.Dispose();
        _channel = null;
    }
}
using System;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Abstractions;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Abstractions;

namespace Mirero.DAQ.Test.Integration.Service;

public static class TestcontainersBuilderExtensions
{
    private const string Daq60DevServerDockerApiEndpoint = "http://192.168.70.37:4243";
    public static ITestcontainersBuilder<TDockerContainer> WithDockerTestEnvironment<TDockerContainer>(this ITestcontainersBuilder<TDockerContainer> me) 
        where TDockerContainer : IDockerContainer?
    {
        var useRemoteContainerServer = (Environment.GetEnvironmentVariable("DAQ_TEST_USE_REMOTE_CONTAINER_SERVER") ?? "0") != "0";
        if (useRemoteContainerServer)
        {
            var remoteContainerServerEndpoint = Environment.GetEnvironmentVariable("DAQ_TEST_REMOTE_CONTAINER_SERVER_ENDPOINT") ??
                                        Daq60DevServerDockerApiEndpoint;
            me = me.WithDockerEndpoint(remoteContainerServerEndpoint);
        }
        return me;
    }

    public static ITestcontainersBuilder<TDockerContainer> WithUserNameSuffixed<TDockerContainer>(
        this ITestcontainersBuilder<TDockerContainer> me, string containerName)
        where TDockerContainer : IDockerContainer?
    {   
        if (!string.IsNullOrWhiteSpace(containerName))
        {
            var userName = Environment.GetEnvironmentVariable("USERNAME") ?? string.Empty;
            me = me.WithName($"{containerName}-{userName}");
        }

        return me;
    }

    public static ITestcontainersBuilder<TDockerContainer> WithDatabaseWithCustomPort<TDockerContainer>(
        this ITestcontainersBuilder<TDockerContainer> builder, TestcontainerDatabaseConfiguration configuration)
        where TDockerContainer : TestcontainerDatabase?
    {
        var customDbPortString = Environment.GetEnvironmentVariable("DAQ_TEST_CONTAINER_DB_SERVER_PORT");
        if (customDbPortString is not null && int.TryParse(customDbPortString, out var port))
        {
            configuration.Port = port;
        }

        return builder.WithDatabase(configuration);
    }
}
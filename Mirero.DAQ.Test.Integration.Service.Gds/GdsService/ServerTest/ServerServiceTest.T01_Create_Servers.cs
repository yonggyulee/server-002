using System;
using System.Linq;
using Grpc.Core;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.ServerTest;

public partial class ServerServiceTest
{
    [Fact]
    public async void T01_Create_Server()
    {
        var superUserOption = await _GetAuthAsync();

        var createServerRequest = new CreateServerRequest
        {
            Id = "Server",
            Address = "192.168.70.37",
            OsType = "CentOS",
            OsVersion = "7",
            CpuCount = 1,
            CpuMemory = 10000
        };

        var createServer = _client.CreateServer(createServerRequest, superUserOption);

        Assert.NotNull(createServer);
        Assert.Equal(createServer.Id, createServerRequest.Id);
        Assert.Equal(createServer.Address, createServerRequest.Address);
        Assert.Equal(createServer.OsType, createServerRequest.OsType);
        Assert.Equal(createServer.OsVersion, createServerRequest.OsVersion);
        Assert.Equal(createServer.CpuCount, createServerRequest.CpuCount);
        Assert.Equal(createServer.CpuMemory, createServerRequest.CpuMemory);

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var testText = new string(Enumerable.Repeat(chars, 257)
            .Select(x => x[random.Next(x.Length)]).ToArray());

        createServerRequest.Id = testText;
        Assert.Throws<RpcException>(() => { _client.CreateServer(createServerRequest, superUserOption); });

        createServerRequest.Id = "Server";
        createServerRequest.OsType = testText;
        Assert.Throws<RpcException>(() => { _client.CreateServer(createServerRequest, superUserOption); });

        createServerRequest.OsType = "CentOS";
        createServerRequest.OsVersion = testText;
        Assert.Throws<RpcException>(() => { _client.CreateServer(createServerRequest, superUserOption); });

        createServerRequest.OsVersion = "7";
        createServerRequest.Address = "192.168.777.999"; 
        Assert.Throws<RpcException>(() => { _client.CreateServer(createServerRequest, superUserOption); });
    }
}
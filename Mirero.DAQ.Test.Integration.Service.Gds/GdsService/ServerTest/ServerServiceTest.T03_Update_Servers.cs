using Grpc.Core;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.ServerTest;

public partial class ServerServiceTest
{
    [Fact]
    public async void T03_Update_Server()
    {
        var superUserOption = await _GetAuthAsync();

        var createServerRequest = new CreateServerRequest
        {
            Id = "UpdateServerTestId",
            Address = "192.168.70.37",
            OsType = "CentOS",
            OsVersion = "7",
            CpuCount = 1,
            CpuMemory = 10000
        };

        _client.CreateServer(createServerRequest, superUserOption);
        var updateServerRequest = new UpdateServerRequest
        {
            Id = "UpdateServerTestId",
            Address = "192.168.70.38",
            OsType = "CentOS",
            OsVersion = "8",
            CpuCount = 3,
            CpuMemory = 60000
        };

        var updateServer = _client.UpdateServer(updateServerRequest, superUserOption);

        Assert.NotNull(updateServer);
        Assert.Equal(updateServerRequest.Id, updateServer.Id);
        Assert.Equal(updateServerRequest.Address, updateServer.Address);
        Assert.Equal(updateServerRequest.OsType, updateServer.OsType);
        Assert.Equal(updateServerRequest.OsVersion, updateServer.OsVersion);
        Assert.Equal(updateServerRequest.CpuCount, updateServer.CpuCount);
        Assert.Equal(updateServerRequest.CpuMemory, updateServer.CpuMemory);

        updateServerRequest.Id = "FailTest";
        Assert.Throws<RpcException>(() => { _client.UpdateServer(updateServerRequest, superUserOption); });

        updateServerRequest.Id = "UpdateServerTestId";
        updateServerRequest.Address = string.Empty;
        Assert.Throws<RpcException>(() => { _client.UpdateServer(updateServerRequest, superUserOption); });
    }
}
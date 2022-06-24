using Grpc.Core;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.ServerTest;

public partial class ServerServiceTest
{
    [Fact]
    public async void T04_Delete_Server()
    {
        var superUserOptions = await _GetAuthAsync();
        var createServerRequest = new CreateServerRequest
        {
            Id = "DeleteServerTestId",
            Address = "192.168.70.30",
            OsType = "CentOS",
            OsVersion = "7",
            CpuCount = 1,
            CpuMemory = 10000
        };

        _client.CreateServer(createServerRequest, superUserOptions);

        var deleteServerRequest = new DeleteServerRequest()
        {
            ServerId = "TestId"
        };

        Assert.Throws<RpcException>(() => { _client.DeleteServer(deleteServerRequest, superUserOptions); });

        deleteServerRequest.ServerId = "DeleteServerTestId";
        _client.DeleteServer(deleteServerRequest, superUserOptions);
    }
}
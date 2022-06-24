using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.ServerService;

public partial class ServerServiceTest
{
    [Fact]
    public async Task T05_Create_Update_Delete_Server()
    {
        var serverName = "Server3";
        var createRequest = new CreateServerRequest()
        {
            Id = serverName,
            Address = "192.168.70.32",
            OsType = "OS",
            OsVersion = "1.0",
            CpuCount = 100,
            CpuMemory = 10000000,
            GpuName = "Gpu",
            GpuCount = 100,
        };

        var createdServer = await _serverServiceClient.CreateServerAsync(createRequest);
        Assert.NotNull(createdServer);

        var updateRequest = new UpdateServerRequest()
        {
            Id = serverName,
            Address = createRequest.Address,
            OsType = createRequest.OsType,
            OsVersion = "1.1",
            CpuCount = createRequest.CpuCount + 100,
            CpuMemory = createRequest.CpuMemory + 100,
            GpuName = createRequest.GpuName,
            GpuCount = createRequest.GpuCount + 100,
        };

        var updatedServer = await _serverServiceClient.UpdateServerAsync(updateRequest);
        Assert.NotNull(updatedServer);
        Assert.Equal(updateRequest.Id, updatedServer.Id);
        Assert.Equal(updateRequest.OsVersion, updatedServer.OsVersion);
        Assert.Equal(updateRequest.CpuCount, updatedServer.CpuCount);
        Assert.Equal(updateRequest.CpuMemory, updatedServer.CpuMemory);
        Assert.Equal(updateRequest.GpuCount, updatedServer.GpuCount);

        await _serverServiceClient.DeleteServerAsync(new DeleteServerRequest() { ServerId = updatedServer.Id});
        
        var listRequest = new ListServersRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id=\"{serverName}\""
            }
        };
        var refreshServers = (await _serverServiceClient.ListServersAsync(listRequest)).Servers;
        Assert.Empty(refreshServers);
    }
}
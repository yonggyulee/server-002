using System.Linq;
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.ServerService;

public partial class ServerServiceTest
{
    [Fact]
    public async Task T04_Create_Get_Delete_Server()
    {
        var serverName = "Server2";
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
        Assert.Equal(createRequest.Id, createdServer.Id);
        Assert.Equal(createRequest.Address, createdServer.Address);
        Assert.Equal(createRequest.OsType, createdServer.OsType);
        Assert.Equal(createRequest.OsVersion, createdServer.OsVersion);
        Assert.Equal(createRequest.CpuCount, createdServer.CpuCount);
        Assert.Equal(createRequest.CpuMemory, createdServer.CpuMemory);
        Assert.Equal(createRequest.GpuName, createdServer.GpuName);
        Assert.Equal(createRequest.GpuCount, createdServer.GpuCount);
        
        var listRequest = new ListServersRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id=\"{serverName}\""
            }
        };

        var servers = (await _serverServiceClient.ListServersAsync(listRequest)).Servers;
        Assert.NotEmpty(servers);

        var target = servers.FirstOrDefault(x => x.Id == createdServer.Id);
        Assert.NotNull(target);
        Assert.Equal(createRequest.Id, target.Id);
        Assert.Equal(createRequest.Address, target.Address);
        Assert.Equal(createRequest.OsType, target.OsType);
        Assert.Equal(createRequest.OsVersion, target.OsVersion);
        Assert.Equal(createRequest.CpuCount, target.CpuCount);
        Assert.Equal(createRequest.CpuMemory, target.CpuMemory);
        Assert.Equal(createRequest.GpuName, target.GpuName);
        Assert.Equal(createRequest.GpuCount, target.GpuCount);
        
        await _serverServiceClient.DeleteServerAsync(new DeleteServerRequest() { ServerId = target.Id});
        var refreshServers = (await _serverServiceClient.ListServersAsync(listRequest)).Servers;
        Assert.Empty(refreshServers);
    }
}
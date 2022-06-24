using System.Linq;
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.ServerService;

public partial class ServerServiceTest
{
    [Fact]
    public async Task T02_Update_Server()
    {
        var serverName = "TestServer";

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

        var target = servers.FirstOrDefault(x => x.Id == serverName);
        Assert.NotNull(target);
        
        var updateRequest = new UpdateServerRequest()
        {
            Id = serverName,
            Address = target.Address,
            OsType = target.OsType,
            OsVersion = "1.1",
            CpuCount = target.CpuCount + 100,
            CpuMemory = target.CpuMemory + 100,
            GpuName = target.GpuName,
            GpuCount = target.GpuCount + 100,
        };

        var updatedServer = await _serverServiceClient.UpdateServerAsync(updateRequest);
        Assert.NotNull(updatedServer);
        Assert.Equal(updateRequest.Id, updatedServer.Id);
        Assert.Equal(updateRequest.OsVersion, updatedServer.OsVersion);
        Assert.Equal(updateRequest.CpuCount, updatedServer.CpuCount);
        Assert.Equal(updateRequest.CpuMemory, updatedServer.CpuMemory);
        Assert.Equal(updateRequest.GpuCount, updatedServer.GpuCount);
    }
}
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.ServerService;

public partial class ServerServiceTest
{
    [Fact]
    public async Task T01_Create_Server()
    {
        var serverName = "Server1";
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
    }
}
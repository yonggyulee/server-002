using System.Linq;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Gds.GdsService.ServerTest;

public partial class ServerServiceTest
{
    [Fact]
    public async void T02_List_Servers()
    {
        var superUserOptions = await _GetAuthAsync();

        var createServerRequest = new CreateServerRequest
        {
            Id = "ListServerTestId",
            Address = "192.168.70.47",
            OsType = "CentOS",
            OsVersion = "7",
            CpuCount = 1,
            CpuMemory = 10000
        };

        _client.CreateServer(createServerRequest, superUserOptions);

        var listServerRequest = new ListServersRequest()
        {
            QueryParameter = new QueryParameter
            {
                PageIndex = 0,
                PageSize = 10
            }
        };

        var listServers = await _client.ListServersAsync(listServerRequest, superUserOptions);
        Assert.NotNull(listServers);
        Assert.Equal(listServers.PageResult.PageIndex, listServerRequest.QueryParameter.PageIndex);
        Assert.Equal(listServers.PageResult.PageSize, listServerRequest.QueryParameter.PageSize);

        var updatedServer = listServers.Servers.SingleOrDefault(x => x.Id == createServerRequest.Id)!;
        Assert.NotNull(updatedServer);
        Assert.Equal(createServerRequest.Address, updatedServer.Address);
        Assert.Equal(createServerRequest.OsType, updatedServer.OsType);
        Assert.Equal(createServerRequest.OsVersion, updatedServer.OsVersion);
        Assert.Equal(createServerRequest.CpuCount, updatedServer.CpuCount);
        Assert.Equal(createServerRequest.CpuMemory, updatedServer.CpuMemory);
    }
}
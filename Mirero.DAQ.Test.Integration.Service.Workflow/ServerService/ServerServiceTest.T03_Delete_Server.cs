using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.ServerService;

public partial class ServerServiceTest
{
    [Fact]
    public async Task T03_Delete_Server()
    {
        var serverName = "TestServer";

        await _serverServiceClient.DeleteServerAsync(new DeleteServerRequest() { ServerId = serverName});
        
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
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkerService;

public partial class WorkerServiceTest
{
    [Fact]
    public async Task T04_Get_Worker_WithServer()
    {
        var listRequest = new ListWorkersRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
            }
        };

        var workers = (await _workerServiceClient.ListWorkersAsync(listRequest)).Workers;
        Assert.All(workers, x =>
        {
            Assert.NotNull(x.Server);
        });
    }
}
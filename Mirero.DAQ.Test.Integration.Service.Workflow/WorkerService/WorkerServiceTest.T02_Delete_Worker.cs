using System.Threading.Tasks;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkerService;

public partial class WorkerServiceTest
{
    [Fact]
    public async Task T02_Delete_Worker()
    {
        var workerName = "TestWorker";
        var deleteRequest = new DeleteWorkerRequest() { WorkerId = workerName };
        await _workerServiceClient.DeleteWorkerAsync(deleteRequest);

        var listRequest = new ListWorkersRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id=\"{workerName}\""
            }
        };

        var workers = (await _workerServiceClient.ListWorkersAsync(listRequest)).Workers;
        Assert.Empty(workers);
    }
}
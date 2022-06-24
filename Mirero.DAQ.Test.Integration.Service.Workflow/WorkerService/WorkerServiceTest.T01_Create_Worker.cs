using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Redis;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkerService;

public partial class WorkerServiceTest
{
    [Fact]
    public async Task T01_Create_Worker()
    {
        var workerName = "Worker1";
        var createRequest = new CreateWorkerRequest()
        {
            Id = workerName,
            ServerId = "TestServer",
            JobType = "JOB_TYPE_TEST",
            CpuCount = 10,
            CpuMemory = 1000000,
            GpuCount = 10,
            GpuMemory = 1000000
        };

        var createdWorker = await _workerServiceClient.CreateWorkerAsync(createRequest);
        
        Assert.NotNull(createdWorker);
        Assert.Equal(createRequest.Id, createdWorker.Id);
        Assert.Equal(createRequest.ServerId, createdWorker.ServerId);
        Assert.Equal(createRequest.JobType, createdWorker.JobType);
        Assert.Equal(createRequest.CpuCount, createdWorker.CpuCount);
        Assert.Equal(createRequest.CpuMemory, createdWorker.CpuMemory);
        Assert.Equal(createRequest.GpuCount, createdWorker.GpuCount);
        Assert.Equal(createRequest.GpuMemory, createdWorker.GpuMemory);

        var database = _fixture.RedisConnection.CreateDatabase();
        var streamName = NameHandler.GetStartJobStreamName(createdWorker.JobType);
        var isExistGroup = await database.KeyExistsAsync(streamName)
                            || (await database.StreamGroupInfoAsync(streamName)).Any(x=>x.Name==streamName);
        
        Assert.True(isExistGroup);
    }
}
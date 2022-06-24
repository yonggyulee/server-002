using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Redis;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.JobService;

public partial class JobServiceTest
{
    [Fact]
    public async Task T01_Create_BatchJob()
    {
        #region signIn
        await _fixture.SignInAsync("administrator", "mirero2816!");
        #endregion
        
        var createRequest = new CreateBatchJobRequest()
        {
            Id = "Test_Lot_1",
            Title = "Test Batch Job",
            TotalCount = 100,
            WorkflowType = WorkflowType.RecipeWorkflow
        };

        var createdBatchJob = await _jobServiceClient.CreateBatchJobAsync(createRequest, _fixture.OptionsWithAuthHeader());
        Assert.NotNull(createdBatchJob);
        Assert.Equal(createRequest.Id, createdBatchJob.Id);
        Assert.Equal(createRequest.Title, createdBatchJob.Title);
        Assert.Equal(createRequest.TotalCount, createdBatchJob.TotalCount);
        Assert.Equal(createRequest.WorkflowType, createdBatchJob.WorkflowType);
        Assert.Equal(_fixture.CurrentTestUser.Name, createdBatchJob.RegisterUser);
        Assert.NotNull(createdBatchJob.RegisterDate);
        Assert.Equal(createdBatchJob.Status, JobStatus.InProgress);
        Assert.Null(createdBatchJob.StartDate);
        Assert.Null(createdBatchJob.EndDate);

        var redisValue = _fixture.RedisConnection.CreateDatabase().StringGet(NameHandler.GetBatchJobStringName(createdBatchJob.Id));
        Assert.Equal((int)redisValue, createdBatchJob.TotalCount);
    }
}
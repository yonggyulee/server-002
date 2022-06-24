using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Redis;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.JobService;

public partial class JobServiceTest
{
    [Fact]
    public async Task T03_Create_Get_Delete_BatchJob()
    {
        #region signIn
        await _fixture.SignInAsync("administrator", "mirero2816!");
        #endregion
        
        var createRequest = new CreateBatchJobRequest()
        {
            Id = "Test_Lot_3",
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

        var listRequest = new ListBatchJobsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id=\"{createdBatchJob.Id}\""
            }
        };

        var batchJobs = (await _jobServiceClient.ListBatchJobsAsync(listRequest)).BatchJobs;
        var targetBatchJob = batchJobs.FirstOrDefault();
        Assert.NotNull(targetBatchJob);
        Assert.Equal(createdBatchJob.Id, targetBatchJob.Id);
        Assert.Equal(createdBatchJob.Title, targetBatchJob.Title);
        Assert.Equal(createdBatchJob.TotalCount, targetBatchJob.TotalCount);
        Assert.Equal(createdBatchJob.WorkflowType, targetBatchJob.WorkflowType);
        Assert.Equal(createdBatchJob.RegisterUser, targetBatchJob.RegisterUser);
        Assert.Equal(createdBatchJob.RegisterDate.TimestampToDateTime(), targetBatchJob.RegisterDate.TimestampToDateTime());
        Assert.Equal(createdBatchJob.Status, targetBatchJob.Status);
        Assert.Equal(createdBatchJob.StartDate?.TimestampToDateTime(), targetBatchJob.StartDate?.TimestampToDateTime());
        Assert.Equal(createdBatchJob.EndDate?.TimestampToDateTime(), targetBatchJob.EndDate?.TimestampToDateTime());

        await _jobServiceClient.DeleteBatchJobAsync(new DeleteBatchJobRequest() { BatchJobId = targetBatchJob.Id });
        var refreshBatchJobs = (await _jobServiceClient.ListBatchJobsAsync(listRequest)).BatchJobs;
        Assert.Empty(refreshBatchJobs);
    }
}
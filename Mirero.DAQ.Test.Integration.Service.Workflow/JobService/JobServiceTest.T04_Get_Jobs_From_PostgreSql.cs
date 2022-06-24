using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.JobService;

public partial class JobServiceTest
{
    [Fact]
    public async Task T04_Get_Jobs_From_PostgreSql()
    {
        var listJobsRequest = new ListJobsRequest()
        {
            QueryParameter = new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10
            },
            BatchJobId = "TestBatchJob2"
        };

        var jobs = (await _jobServiceClient.ListJobsAsync(listJobsRequest)).Jobs;
        Assert.NotEmpty(jobs);

        var batchJobId = jobs.FirstOrDefault().BatchJobId;
        var listBatchJobsRequest = new ListBatchJobsRequest()
        {
            QueryParameter = new QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id=\"{batchJobId}\""
            }
        };
        
    }
}
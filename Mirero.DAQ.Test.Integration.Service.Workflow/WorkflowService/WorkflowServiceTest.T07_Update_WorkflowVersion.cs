using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T07_Update_WorkflowVersion()
    {
        #region signIn
        await _fixture.SignInAsync("administrator", "mirero2816!");
        #endregion
        
        var listRequest = new ListWorkflowVersionsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
            },
            WorkflowId = 2
        };
        var workflowVersions = (await _workflowServiceClient.ListWorkflowVersionsAsync(listRequest)).WorkflowVersions;
        var target = workflowVersions.FirstOrDefault();
        
        var updateRequest = new UpdateWorkflowVersionRequest()
        {
            Id = target.Id,
            WorkflowId = target.WorkflowId,
            FileName = $"{target.FileName}_Updated",
        };

        //update속도가 너무 빠르면 시간 비교가 힘들어서 시간줌
        Thread.Sleep(1000);
        
        var updatedWorkflowVersion = await _workflowServiceClient.UpdateWorkflowVersionAsync(updateRequest, _fixture.OptionsWithAuthHeader());
        Assert.NotNull(updatedWorkflowVersion);
        Assert.Equal(updateRequest.FileName, updatedWorkflowVersion.FileName);
        Assert.Equal(target.CreateDate.TimestampToDateTime(), updatedWorkflowVersion.CreateDate.TimestampToDateTime());
        Assert.Equal(target.CreateUser, updatedWorkflowVersion.CreateUser);
        Assert.NotEqual(target.UpdateDate.TimestampToDateTime(), updatedWorkflowVersion.UpdateDate.TimestampToDateTime());
        Assert.Equal(_fixture.CurrentTestUser.Name, updatedWorkflowVersion.UpdateUser);
    }
}
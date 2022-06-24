using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T10_Create_Update_Delete_WorkflowVersion()
    {
        #region signIn
        var user = await _fixture.SignInAsync("administrator", "mirero2816!");
        #endregion
        
        var creteRequest = new CreateWorkflowVersionRequest()
        {
            WorkflowId = 2,
            Version = "1.0",
            FileName = "Test.py",
        };

        var createdWorkflowVersion = await _workflowServiceClient.CreateWorkflowVersionAsync(creteRequest, _fixture.OptionsWithAuthHeader());
        Assert.NotNull(createdWorkflowVersion);
        Assert.NotEqual(0, createdWorkflowVersion.Id);
        Assert.Equal(creteRequest.WorkflowId, createdWorkflowVersion.WorkflowId);
        Assert.Equal(creteRequest.Version, createdWorkflowVersion.Version);
        Assert.Equal(creteRequest.FileName, createdWorkflowVersion.FileName);
        Assert.Equal(_fixture.CurrentTestUser.Name, createdWorkflowVersion.CreateUser);
        
        var updateRequest = new UpdateWorkflowVersionRequest()
        {
            Id = createdWorkflowVersion.Id,
            WorkflowId = createdWorkflowVersion.WorkflowId,
            FileName = $"{createdWorkflowVersion.FileName}_Updated",
        };

        //update속도가 너무 빠르면 시간 비교가 힘들어서 시간줌
        Thread.Sleep(1000);
        
        var updatedWorkflowVersion = await _workflowServiceClient.UpdateWorkflowVersionAsync(updateRequest, _fixture.OptionsWithAuthHeader());
        Assert.NotNull(updatedWorkflowVersion);
        Assert.Equal(updateRequest.FileName, updatedWorkflowVersion.FileName);
        Assert.Equal(createdWorkflowVersion.CreateDate.TimestampToDateTime(), updatedWorkflowVersion.CreateDate.TimestampToDateTime());
        Assert.Equal(createdWorkflowVersion.CreateUser, updatedWorkflowVersion.CreateUser);
        Assert.NotEqual(createdWorkflowVersion.UpdateDate.TimestampToDateTime(), updatedWorkflowVersion.UpdateDate.TimestampToDateTime());
        Assert.Equal(_fixture.CurrentTestUser.Name, updatedWorkflowVersion.UpdateUser);

        await _workflowServiceClient.DeleteWorkflowVersionAsync(new DeleteWorkflowVersionRequest()
            { WorkflowVersionId = updatedWorkflowVersion.Id });
        
        var listRequest = new ListWorkflowVersionsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id={updatedWorkflowVersion.Id}"
            },
            WorkflowId = updatedWorkflowVersion.WorkflowId
        };

        var workflowVersions = (await _workflowServiceClient.ListWorkflowVersionsAsync(listRequest)).WorkflowVersions;
        Assert.Empty(workflowVersions);
    }
}
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T05_Create_Update_Delete_Workflow()
    {
        #region signIn
        var user = await _fixture.SignInAsync("administrator", "mirero2816!");
        #endregion
        
        var createRequest = new CreateWorkflowRequest()
        {
            VolumeId = "TestVolume",
            Type = WorkflowType.RecipeWorkflow,
            Title = $"Test Workflow{DateTime.Now}"
        };

        var createdWorkflow = await _workflowServiceClient.CreateWorkflowAsync(createRequest, _fixture.OptionsWithAuthHeader());
        Assert.NotNull(createdWorkflow);
        Assert.NotEqual(0, createdWorkflow.Id);
        Assert.Equal(createRequest.Type, createdWorkflow.Type);
        Assert.Equal(createRequest.Title, createdWorkflow.Title);
        Assert.Equal(createRequest.VolumeId, createdWorkflow.VolumeId);
        Assert.Equal(_fixture.CurrentTestUser.Name, createdWorkflow.CreateUser);

        var updateRequest = new UpdateWorkflowRequest()
        {
            Id = createdWorkflow.Id,
            VolumeId = createdWorkflow.VolumeId,
            Type = WorkflowType.PipelineWorkflow,
            Title = $"Test Workflow{DateTime.Now}"
        };

        //update속도가 너무 빠르면 시간 비교가 힘들어서 시간줌
        Thread.Sleep(1000);
        
        var updatedWorkflow = await _workflowServiceClient.UpdateWorkflowAsync(updateRequest, _fixture.OptionsWithAuthHeader());
        Assert.NotNull(updatedWorkflow);
        Assert.Equal(updateRequest.Id, updatedWorkflow.Id);
        Assert.Equal(updateRequest.Type, updatedWorkflow.Type);
        Assert.Equal(updateRequest.Title, updatedWorkflow.Title);
        Assert.Equal(updateRequest.VolumeId, updatedWorkflow.VolumeId);
        Assert.Equal(updateRequest.Type, updatedWorkflow.Type);
        Assert.Equal(createdWorkflow.CreateDate.TimestampToDateTime(), updatedWorkflow.CreateDate.TimestampToDateTime());
        Assert.Equal(createdWorkflow.CreateUser, updatedWorkflow.CreateUser);
        Assert.Equal(_fixture.CurrentTestUser.Name, updatedWorkflow.UpdateUser);
        Assert.NotEqual(createdWorkflow.UpdateDate.TimestampToDateTime(), updatedWorkflow.UpdateDate.TimestampToDateTime());

        await _workflowServiceClient.DeleteWorkflowAsync(
            new DeleteWorkflowRequest() { WorkflowId = updatedWorkflow.Id });

        var listRequest = new ListWorkflowsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id=\"{updatedWorkflow.Id}\""
            }
        };
        
        var workflows = (await _workflowServiceClient.ListWorkflowsAsync(listRequest)).Workflows;
        Assert.Empty(workflows);
    }
}
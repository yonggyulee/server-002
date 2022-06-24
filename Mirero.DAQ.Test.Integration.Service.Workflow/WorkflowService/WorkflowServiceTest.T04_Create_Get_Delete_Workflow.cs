using System.Linq;
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T04_Create_Get_Delete_Workflow()
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

        var listRequest = new ListWorkflowsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id=\"{createdWorkflow.Id}\""
            }
        };

        var workflows = (await _workflowServiceClient.ListWorkflowsAsync(listRequest)).Workflows;
        Assert.NotEmpty(workflows);

        var target = workflows.FirstOrDefault();
        Assert.NotNull(target);
        Assert.Equal(createdWorkflow.Id, target.Id);
        Assert.Equal(createdWorkflow.Type, target.Type);
        Assert.Equal(createdWorkflow.Title, target.Title);
        Assert.Equal(createdWorkflow.VolumeId, target.VolumeId);
        Assert.Equal(createdWorkflow.CreateUser, target.CreateUser);
        Assert.Equal(createdWorkflow.CreateDate.TimestampToDateTime(), target.CreateDate.TimestampToDateTime());

        await _workflowServiceClient.DeleteWorkflowAsync(new DeleteWorkflowRequest() { WorkflowId = target.Id });
        
        var refreshWorkflows = (await _workflowServiceClient.ListWorkflowsAsync(listRequest)).Workflows;
        Assert.Empty(refreshWorkflows);
    }
}
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T01_Create_Workflow()
    {
        #region signIn
        var user = await _fixture.SignInAsync("administrator", "mirero2816!");
        #endregion

        var createRequest = new CreateWorkflowRequest()
        {
            VolumeId = "TestVolume",
            Type = WorkflowType.RecipeWorkflow,
            Title = "It is Test Workflow"
        };

        var createdWorkflow = await _workflowServiceClient.CreateWorkflowAsync(createRequest, _fixture.OptionsWithAuthHeader());
        Assert.NotNull(createdWorkflow);
        Assert.NotEqual(0, createdWorkflow.Id);
        Assert.Equal(createRequest.Type, createdWorkflow.Type);
        Assert.Equal(createRequest.Title, createdWorkflow.Title);
        Assert.Equal(createRequest.VolumeId, createdWorkflow.VolumeId);
        Assert.Equal(_fixture.CurrentTestUser.Name, createdWorkflow.CreateUser);
    }
}
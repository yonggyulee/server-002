using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T11_Create_WorkflowVersion_Set_DefaultWorkflowVersion()
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

        await _workflowServiceClient.SetDefaultWorkflowVersionAsync(
            new SetDefaultWorkflowVersionRequest()
                { WorkflowId = createdWorkflowVersion.WorkflowId, WorkflowVersionId = createdWorkflowVersion.Id});
        
        var listRequest = new ListWorkflowsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id=\"{createdWorkflowVersion.WorkflowId}\""
            }
        };

        var workflows = (await _workflowServiceClient.ListWorkflowsAsync(listRequest)).Workflows;
        var targetWorkflow = workflows.FirstOrDefault();
        Assert.NotNull(targetWorkflow);
        Assert.Equal(createdWorkflowVersion.Id, targetWorkflow.DefaultWorkflowVersionId);
    }
}
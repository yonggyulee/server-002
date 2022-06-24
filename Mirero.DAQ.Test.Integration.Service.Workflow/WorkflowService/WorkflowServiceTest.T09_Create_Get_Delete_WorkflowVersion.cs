using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T09_Create_Get_Delete_WorkflowVersion()
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
        
        var listRequest = new ListWorkflowVersionsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id={createdWorkflowVersion.Id}"
            },
            WorkflowId = createdWorkflowVersion.WorkflowId
        };

        var workflowVersions = (await _workflowServiceClient.ListWorkflowVersionsAsync(listRequest)).WorkflowVersions;
        Assert.NotEmpty(workflowVersions);

        var target = workflowVersions.FirstOrDefault();
        Assert.NotNull(target);
        Assert.Equal(createdWorkflowVersion.Id, target.Id);
        Assert.Equal(createdWorkflowVersion.WorkflowId, target.WorkflowId);
        Assert.Equal(createdWorkflowVersion.Version, target.Version);
        Assert.Equal(createdWorkflowVersion.FileName, target.FileName);
        Assert.Equal(createdWorkflowVersion.CreateUser, target.CreateUser);
        Assert.Equal(createdWorkflowVersion.CreateDate.TimestampToDateTime(), target.CreateDate.TimestampToDateTime());

        await _workflowServiceClient.DeleteWorkflowVersionAsync(new DeleteWorkflowVersionRequest(){ WorkflowVersionId = target.Id });
        
        var refreshWorkflowVersions = (await _workflowServiceClient.ListWorkflowVersionsAsync(listRequest)).WorkflowVersions;
        Assert.Empty(refreshWorkflowVersions);
    }
}
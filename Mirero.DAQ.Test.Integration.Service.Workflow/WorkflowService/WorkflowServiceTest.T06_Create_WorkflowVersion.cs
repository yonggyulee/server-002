using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T06_Create_WorkflowVersion()
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
        Assert.Equal(creteRequest.WorkflowId, createdWorkflowVersion.WorkflowId);
        Assert.Equal(creteRequest.FileName, createdWorkflowVersion.FileName);
        Assert.Equal(_fixture.CurrentTestUser.Name, createdWorkflowVersion.CreateUser);
    }
}
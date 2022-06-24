using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T08_Delete_WorkflowVersion()
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
        
        var deleteRequest = new DeleteWorkflowVersionRequest() { WorkflowVersionId = target.Id };
        await _workflowServiceClient.DeleteWorkflowVersionAsync(deleteRequest);
        
        listRequest.QueryParameter.Where = $"Id={target.Id}";
        var refreshWorkflowVersions = (await _workflowServiceClient.ListWorkflowVersionsAsync(listRequest)).WorkflowVersions;
        Assert.Empty(refreshWorkflowVersions);
    }
}
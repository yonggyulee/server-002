using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T14_Delete_Workflow_Check_WorkflowVersion()
    {
        await _workflowServiceClient.DeleteWorkflowAsync(new DeleteWorkflowRequest() { WorkflowId = 1 });
       
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
        Assert.Empty(workflowVersions);
    }
}
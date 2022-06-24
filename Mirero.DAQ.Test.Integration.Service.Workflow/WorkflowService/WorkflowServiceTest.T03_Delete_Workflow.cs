using System.Linq;
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T03_Delete_Workflow()
    {
        var listRequest = new ListWorkflowsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
            }
        };
        var workflows = (await _workflowServiceClient.ListWorkflowsAsync(listRequest)).Workflows;
        var target = workflows.FirstOrDefault();
        
        var deleteRequest = new DeleteWorkflowRequest() { WorkflowId = target.Id };
        await _workflowServiceClient.DeleteWorkflowAsync(deleteRequest);
        
        listRequest.QueryParameter.Where = $"Id={target.Id}";
        var refreshWorkflows = (await _workflowServiceClient.ListWorkflowsAsync(listRequest)).Workflows;
        Assert.Empty(refreshWorkflows);
    }
}
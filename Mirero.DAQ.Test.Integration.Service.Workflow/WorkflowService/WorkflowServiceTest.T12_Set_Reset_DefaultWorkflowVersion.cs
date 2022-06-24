using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T12_Set_Reset_DefaultWorkflowVersion()
    {
        var workflowVersionListRequest = new ListWorkflowVersionsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
            },
            WorkflowId = 2
        };
        var workflowVersions = (await _workflowServiceClient.ListWorkflowVersionsAsync(workflowVersionListRequest)).WorkflowVersions;
        var targetWorkflowVersion = workflowVersions.FirstOrDefault();
        
        await _workflowServiceClient.SetDefaultWorkflowVersionAsync(
            new SetDefaultWorkflowVersionRequest()
                { WorkflowId = targetWorkflowVersion.WorkflowId, WorkflowVersionId = targetWorkflowVersion.Id});

        var workflowListRequest = new ListWorkflowsRequest()
        {
            QueryParameter = new Domain.Common.Protos.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 10,
                Where = $"Id=\"{targetWorkflowVersion.WorkflowId}\""
            }
        };
        
        var workflows = (await _workflowServiceClient.ListWorkflowsAsync(workflowListRequest)).Workflows;
        var targetWorkflow = workflows.FirstOrDefault();
        Assert.NotNull(targetWorkflow);
        Assert.Equal(targetWorkflowVersion.Id, targetWorkflow.DefaultWorkflowVersionId);


        await _workflowServiceClient.ResetDefaultWorkflowVersionAsync(
            new ResetDefaultWorkflowVersionRequest() { WorkflowId = targetWorkflowVersion.Id });
        
        var refreshWorkflows = (await _workflowServiceClient.ListWorkflowsAsync(workflowListRequest)).Workflows;
        var refreshWorkflow = refreshWorkflows.FirstOrDefault();
        Assert.NotNull(refreshWorkflow);
        Assert.Null(refreshWorkflow.DefaultWorkflowVersionId);
    }
}
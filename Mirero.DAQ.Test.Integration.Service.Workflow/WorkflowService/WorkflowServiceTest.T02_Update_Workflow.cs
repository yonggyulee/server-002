using System.Linq;
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Workflow.WorkflowService;

public partial class WorkflowServiceTest
{
    [Fact]
    public async Task T02_Update_Workflow()
    {
        #region signIn
        await _fixture.SignInAsync("administrator", "mirero2816!");
        #endregion
        
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
        
        var updateRequest = new UpdateWorkflowRequest()
        {
            Id = target.Id,
            VolumeId = target.VolumeId,
            Title = $"{target.Title}_Updated",
            Type = WorkflowType.TrainWorkflow
        };
        
        //update속도가 너무 빠르면 시간 비교가 힘들어서 시간줌
        Thread.Sleep(1000);
        
        var updatedWorkflow = await _workflowServiceClient.UpdateWorkflowAsync(updateRequest, _fixture.OptionsWithAuthHeader());
        Assert.NotNull(updatedWorkflow);
        Assert.Equal(updateRequest.Title, updatedWorkflow.Title);
        Assert.Equal(updateRequest.Type, updatedWorkflow.Type);
        Assert.Equal(target.CreateDate.TimestampToDateTime(), updatedWorkflow.CreateDate.TimestampToDateTime());
        Assert.Equal(target.CreateUser, updatedWorkflow.CreateUser);
        Assert.Equal(_fixture.CurrentTestUser.Name, updatedWorkflow.UpdateUser);
        Assert.NotEqual(target.UpdateDate.TimestampToDateTime(), updatedWorkflow.UpdateDate.TimestampToDateTime());
    }
}
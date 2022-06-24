using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.ListWorkflows;

public class ListWorkflowsCommand : IRequest<ListWorkflowsResponse>
{
    public ListWorkflowsRequest Request { get; private set; }
    public ListWorkflowsCommand(ListWorkflowsRequest request)
    {
        Request = request;
    }
}

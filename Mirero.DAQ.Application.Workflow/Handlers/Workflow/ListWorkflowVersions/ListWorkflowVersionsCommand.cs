using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.ListWorkflowVersions;

public class ListWorkflowVersionsCommand : IRequest<ListWorkflowVersionsResponse>
{
    public ListWorkflowVersionsRequest Request { get; set; }

    public ListWorkflowVersionsCommand(ListWorkflowVersionsRequest request)
    {
        Request = request;
    }
}

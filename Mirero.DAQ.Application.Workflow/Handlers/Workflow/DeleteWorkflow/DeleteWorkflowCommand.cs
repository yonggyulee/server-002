using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.DeleteWorkflow;

public class DeleteWorkflowCommand : IRequest<Empty>
{
    public DeleteWorkflowRequest Request { get; set; }

    public DeleteWorkflowCommand(DeleteWorkflowRequest request)
    {
        Request = request;
    }
}

using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.DeleteWorkflowVersion;

public class DeleteWorkflowVersionCommand : IRequest<Empty>
{
    public DeleteWorkflowVersionRequest Request { get; private set; }

    public DeleteWorkflowVersionCommand(DeleteWorkflowVersionRequest request)
    {
        Request = request;
    }
}

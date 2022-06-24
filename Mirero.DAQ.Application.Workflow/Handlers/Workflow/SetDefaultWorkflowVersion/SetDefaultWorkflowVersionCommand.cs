using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.SetDefaultWorkflowVersion;

public class SetDefaultWorkflowVersionCommand : IRequest<Empty>
{
    public SetDefaultWorkflowVersionRequest Request { get; private set; }

    public SetDefaultWorkflowVersionCommand(SetDefaultWorkflowVersionRequest request)
    {
        Request = request;
    }
}

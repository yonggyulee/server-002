using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.ResetDefaultWorkflowVersion;

public class ResetDefaultWorkflowVersionCommand : IRequest<Empty>
{
    public ResetDefaultWorkflowVersionRequest Request { get; set; }

    public ResetDefaultWorkflowVersionCommand(ResetDefaultWorkflowVersionRequest request)
    {
        Request = request;
    }
}

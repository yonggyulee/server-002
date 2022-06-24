using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.UpdateWorkflowVersion;

public class UpdateWorkflowVersionCommand : IRequest<WorkflowVersion>
{
    public UpdateWorkflowVersionRequest Request { get; private set; }

    public UpdateWorkflowVersionCommand(UpdateWorkflowVersionRequest request)
    {
        Request = request;
    }
}

using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.CreateWorkflowVersion;

public class CreateWorkflowVersionCommand : IRequest<Domain.Workflow.Protos.V1.WorkflowVersion>
{
    public CreateWorkflowVersionRequest Request { get; set; }

    public CreateWorkflowVersionCommand(CreateWorkflowVersionRequest request)
    {
        Request = request;
    }
}

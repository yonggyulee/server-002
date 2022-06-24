using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.CreateWorkflow;

public class CreateWorkflowCommand : IRequest<Domain.Workflow.Protos.V1.Workflow>
{
    public CreateWorkflowRequest Request { get; private set; }
    public CreateWorkflowCommand(CreateWorkflowRequest request)
    {
        Request = request;
    }
}

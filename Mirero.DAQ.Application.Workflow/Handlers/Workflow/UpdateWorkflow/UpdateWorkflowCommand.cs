using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.UpdateWorkflow;

public class UpdateWorkflowCommand : IRequest<Domain.Workflow.Protos.V1.Workflow> 
{ 
    public UpdateWorkflowRequest Request { get; private set; }

    public UpdateWorkflowCommand(UpdateWorkflowRequest request)
    {
        Request = request;
    }
}

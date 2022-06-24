using Google.Protobuf.WellKnownTypes;
using MediatR;

namespace Mirero.DAQ.Application.Workflow.Handlers.Worker.StartWorker;

public class StartWorkerHandler : IRequestHandler<StartWorkerCommand, Empty>
{
    public StartWorkerHandler() 
    {
    }

    public Task<Empty> Handle(StartWorkerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        throw new NotImplementedException();
    }
}

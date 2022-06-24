using Google.Protobuf.WellKnownTypes;
using MediatR;

namespace Mirero.DAQ.Application.Workflow.Handlers.Worker.StopWorker;

public class StopWorkerHandler : IRequestHandler<StopWorkerCommand, Empty>
{
    public StopWorkerHandler() 
    {
    }

    public Task<Empty> Handle(StopWorkerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        throw new NotImplementedException();
    }
}

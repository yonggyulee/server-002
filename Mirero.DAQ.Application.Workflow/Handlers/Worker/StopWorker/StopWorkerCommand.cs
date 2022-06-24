using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Worker.StopWorker;

public class StopWorkerCommand : IRequest<Empty>
{
    public StopWorkerRequest Request { get; private set; }

    public StopWorkerCommand(StopWorkerRequest request)
    {
        Request = request;
    }
}

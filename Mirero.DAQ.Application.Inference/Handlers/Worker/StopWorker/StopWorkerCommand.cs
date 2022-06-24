using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Worker.StopWorker;

public class StopWorkerCommand : IRequest<Empty>
{
    public StopWorkerRequest Request { get; set; }

    public StopWorkerCommand(StopWorkerRequest request)
    {
        Request = request;
    }
}
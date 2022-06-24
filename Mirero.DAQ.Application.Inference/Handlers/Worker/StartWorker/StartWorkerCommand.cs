using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Worker.StartWorker;

public class StartWorkerCommand : IRequest<Empty>
{
    public StartWorkerRequest Request { get; set; }

    public StartWorkerCommand(StartWorkerRequest request)
    {
        Request = request;
    }
}
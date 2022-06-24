using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Worker.RemoveWorker;

public class RemoveWorkerCommand : IRequest<Empty>
{
    public RemoveWorkerRequest Request { get; set; }

    public RemoveWorkerCommand(RemoveWorkerRequest request)
    {
        Request = request;
    }
}
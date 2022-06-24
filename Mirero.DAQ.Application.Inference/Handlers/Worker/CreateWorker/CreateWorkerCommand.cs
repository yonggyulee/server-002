using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Worker.CreateWorker;

public class CreateWorkerCommand : IRequest<Empty>
{
    public CreateWorkerRequest Request { get; set; }

    public CreateWorkerCommand(CreateWorkerRequest request)
    {
        Request = request;
    }
}
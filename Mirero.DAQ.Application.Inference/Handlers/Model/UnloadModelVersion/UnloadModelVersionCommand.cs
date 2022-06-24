using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.UnloadModelVersion;

public class UnloadModelVersionCommand : IRequest<Empty>
{
    public UnloadModelVersionRequest Request { get; set; }

    public UnloadModelVersionCommand(UnloadModelVersionRequest request)
    {
        Request = request;
    }
}
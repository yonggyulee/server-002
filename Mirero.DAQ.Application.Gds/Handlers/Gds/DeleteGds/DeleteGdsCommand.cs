using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.DeleteGds;

public class DeleteGdsCommand : IRequest<Empty>
{
    public DeleteGdsRequest Request { get; }

    public DeleteGdsCommand(DeleteGdsRequest request)
    {
        Request = request;
    }
}
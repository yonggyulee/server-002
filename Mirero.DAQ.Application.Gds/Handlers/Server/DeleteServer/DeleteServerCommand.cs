using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Server.DeleteServer;

public class DeleteServerCommand : IRequest<Empty>
{
    public DeleteServerRequest Request { get; }

    public DeleteServerCommand(DeleteServerRequest request)
    {
        Request = request;
    }
}

using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Server.UpdateServer;

public class UpdateServerCommand : IRequest<Domain.Gds.Protos.V1.Server>
{
    public UpdateServerRequest Request { get; }

    public UpdateServerCommand(UpdateServerRequest request)
    {
        Request = request;
    }
}

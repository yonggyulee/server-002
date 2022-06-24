using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Server.CreateServer;

public class CreateServerCommand : IRequest<Domain.Gds.Protos.V1.Server>
{
    public CreateServerRequest Request { get; }

    public CreateServerCommand(CreateServerRequest request)
    {
        Request = request;
    }
}

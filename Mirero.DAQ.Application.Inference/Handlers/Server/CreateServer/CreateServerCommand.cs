using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Server.CreateServer;

public class CreateServerCommand : IRequest<Domain.Inference.Protos.V1.Server>
{
    public CreateServerRequest Request { get; set; }

    public CreateServerCommand(CreateServerRequest request)
    {
        Request = request;
    }
}
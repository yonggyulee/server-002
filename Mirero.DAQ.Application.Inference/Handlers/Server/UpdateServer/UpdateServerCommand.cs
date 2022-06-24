using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Server.UpdateServer;

public class UpdateServerCommand : IRequest<Domain.Inference.Protos.V1.Server>
{
    public UpdateServerRequest Request { get; set; }

    public UpdateServerCommand(UpdateServerRequest request)
    {
        Request = request;
    }
}
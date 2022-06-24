using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Server.DeleteServer;

public class DeleteServerCommand : IRequest<Domain.Inference.Protos.V1.Server>
{
    public DeleteServerRequest Request { get; set; }

    public DeleteServerCommand(DeleteServerRequest request)
    {
        Request = request;
    }
}
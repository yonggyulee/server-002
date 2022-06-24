using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Server.ListServers;

public class ListServersCommand : IRequest<ListServersResponse>
{
    public ListServersRequest Request { get; set; }

    public ListServersCommand(ListServersRequest request)
    {
        Request = request;
    }
}
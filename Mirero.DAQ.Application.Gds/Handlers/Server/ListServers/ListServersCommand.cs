using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Server.ListServers;

public class ListServersCommand : IRequest<ListServersResponse>
{
    public ListServersRequest Request { get; }

    public ListServersCommand(ListServersRequest request)
    {
        Request = request;
    }
}

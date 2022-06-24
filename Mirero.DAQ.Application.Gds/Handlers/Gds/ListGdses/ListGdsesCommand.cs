using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.ListGdses;

public class ListGdsesCommand : IRequest<ListGdsesResponse>
{
    public ListGdsesRequest Request { get; }

    public ListGdsesCommand(ListGdsesRequest request)
    {
        Request = request;
    }
}
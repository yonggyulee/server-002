using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Volume.ListVolumes;

public class ListVolumesCommand : IRequest<ListVolumesResponse>
{
    public ListVolumesRequest Request { get; }

    public ListVolumesCommand(ListVolumesRequest request)
    {
        Request = request;
    }
}
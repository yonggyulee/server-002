using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.Volume.ListVolumes;

public class ListVolumesCommand : IRequest<ListVolumesResponse>
{
    public ListVolumesRequest Request { get; set; }

    public ListVolumesCommand(ListVolumesRequest request)
    {
        Request = request;
    }
}
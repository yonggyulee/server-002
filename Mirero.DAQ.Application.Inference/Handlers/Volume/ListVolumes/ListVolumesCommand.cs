using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Volume.ListVolumes;

public class ListVolumesCommand : IRequest<ListVolumesResponse>
{
    public ListVolumesRequest Request { get; set; }

    public ListVolumesCommand(ListVolumesRequest request)
    {
        Request = request;
    }
}
using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.Volume.GetVolume;

public class GetVolumeCommand : IRequest<Domain.Dataset.Protos.V1.Volume>
{
    public GetVolumeRequest Request { get; set; }

    public GetVolumeCommand(GetVolumeRequest request)
    {
        Request = request;
    }
}
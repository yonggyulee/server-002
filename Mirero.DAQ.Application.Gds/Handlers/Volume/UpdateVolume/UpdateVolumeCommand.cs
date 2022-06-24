using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Volume.UpdateVolume;

public class UpdateVolumeCommand : IRequest<Domain.Gds.Protos.V1.Volume>
{
    public UpdateVolumeRequest Request { get; }

    public UpdateVolumeCommand(UpdateVolumeRequest request)
    {
        Request = request;
    }
}
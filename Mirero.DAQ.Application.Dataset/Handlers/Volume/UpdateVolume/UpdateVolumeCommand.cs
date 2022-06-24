using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.Volume.UpdateVolume;

public class UpdateVolumeCommand : IRequest<Domain.Dataset.Protos.V1.Volume>
{
    public UpdateVolumeRequest Request { get; set; }

    public UpdateVolumeCommand(UpdateVolumeRequest request)
    {
        Request = request;
    }
}
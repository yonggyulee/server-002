using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Volume.UpdateVolume;

public class UpdateVolumeCommand : IRequest<Domain.Inference.Protos.V1.Volume>
{
    public UpdateVolumeRequest Request { get; set; }

    public UpdateVolumeCommand(UpdateVolumeRequest request)
    {
        Request = request;
    }
}
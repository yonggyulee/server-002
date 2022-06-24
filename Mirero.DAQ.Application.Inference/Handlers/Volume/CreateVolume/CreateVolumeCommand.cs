using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Volume.CreateVolume;

public class CreateVolumeCommand : IRequest<Domain.Inference.Protos.V1.Volume>
{
    public CreateVolumeRequest Request { get; set; }

    public CreateVolumeCommand(CreateVolumeRequest request)
    {
        Request = request;
    }
}
using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Volume.CreateVolume;

public class CreateVolumeCommand : IRequest<Domain.Gds.Protos.V1.Volume>
{
    public CreateVolumeRequest Request { get; }

    public CreateVolumeCommand(CreateVolumeRequest request)
    {
        Request = request;
    }
}
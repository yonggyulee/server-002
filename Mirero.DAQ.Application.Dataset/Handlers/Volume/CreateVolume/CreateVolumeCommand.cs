using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.Volume.CreateVolume;

public class CreateVolumeCommand : IRequest<Domain.Dataset.Protos.V1.Volume>
{
    public CreateVolumeRequest Request { get; set; }

    public CreateVolumeCommand(CreateVolumeRequest request)
    {
        Request = request;
    }
}
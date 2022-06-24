using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Volume.DeleteVolume;

public class DeleteVolumeCommand : IRequest<Domain.Inference.Protos.V1.Volume>
{
    public DeleteVolumeRequest Request { get; set; }

    public DeleteVolumeCommand(DeleteVolumeRequest request)
    {
        Request = request;
    }
}
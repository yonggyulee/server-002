using MediatR;
using Mirero.DAQ.Domain.Dataset.Protos.V1;

namespace Mirero.DAQ.Application.Dataset.Handlers.Volume.DeleteVolume;

public class DeleteVolumeCommand : IRequest<Domain.Dataset.Protos.V1.Volume>
{
    public DeleteVolumeRequest Request { get; set; }

    public DeleteVolumeCommand(DeleteVolumeRequest request)
    {
        Request = request;
    }
}
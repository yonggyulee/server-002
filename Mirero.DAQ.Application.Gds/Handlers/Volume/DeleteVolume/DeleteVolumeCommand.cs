using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Volume.DeleteVolume;

public class DeleteVolumeCommand : IRequest<Empty>
{
    public DeleteVolumeRequest Request { get; }

    public DeleteVolumeCommand(DeleteVolumeRequest request)
    {
        Request = request;
    }
}
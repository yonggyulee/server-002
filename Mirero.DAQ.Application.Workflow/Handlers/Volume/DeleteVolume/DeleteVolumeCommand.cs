using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Volume.DeleteVolume;

public class DeleteVolumeCommand : IRequest<Empty>
{
    public DeleteVolumeRequest Request { get; private set; }

    public DeleteVolumeCommand(DeleteVolumeRequest request)
    {
        Request = request;
    }
}

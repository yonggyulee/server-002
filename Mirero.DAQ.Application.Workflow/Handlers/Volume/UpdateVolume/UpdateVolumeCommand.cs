using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Volume.UpdateVolume;

public class UpdateVolumeCommand : IRequest<Domain.Workflow.Protos.V1.Volume>
{
    public UpdateVolumeRequest Request { get; private set; }
    public UpdateVolumeCommand(UpdateVolumeRequest request)
    {
        Request = request;
    }
}

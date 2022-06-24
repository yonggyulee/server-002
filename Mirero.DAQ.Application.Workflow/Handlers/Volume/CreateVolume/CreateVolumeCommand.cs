using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Volume.CreateVolume;

public class CreateVolumeCommand : IRequest<Domain.Workflow.Protos.V1.Volume>
{
    public CreateVolumeRequest Request { get; private set; }

    public CreateVolumeCommand(CreateVolumeRequest request)
    {
        Request = request;
    }
}

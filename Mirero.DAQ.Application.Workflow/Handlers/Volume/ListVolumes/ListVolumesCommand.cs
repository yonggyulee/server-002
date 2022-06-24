using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Volume.ListVolumes;

public class ListVolumesCommand : IRequest<ListVolumesResponse>
{
    public ListVolumesRequest Request { get; private set; }

    public ListVolumesCommand(ListVolumesRequest request)
    {
        Request = request;
    }
}

using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Server.UpdateServer;

public class UpdateServerCommand : IRequest<Domain.Workflow.Protos.V1.Server>
{
    public UpdateServerRequest Request { get; private set; }

    public UpdateServerCommand(UpdateServerRequest request)
    {
        Request = request;
    }
}

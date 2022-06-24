using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Server.DeleteServer;

public class DeleteServerCommand : IRequest<Empty>
{
    public DeleteServerRequest Request { get; private set; }

    public DeleteServerCommand(DeleteServerRequest request)
    {
        Request = request;
    }
}

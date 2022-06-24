using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Server.CreateServer;

public class CreateServerCommand : IRequest<Domain.Workflow.Protos.V1.Server>
{
    public CreateServerRequest Request { get; private set; }

    public CreateServerCommand(CreateServerRequest request)
    {
        Request = request;
    }
}

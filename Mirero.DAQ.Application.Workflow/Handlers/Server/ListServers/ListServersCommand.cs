using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Server.ListServers;

public class ListServersCommand : IRequest<ListServersResponse>
{
    public ListServersRequest Request { get; private set; }

    public ListServersCommand(ListServersRequest request)
    {
        Request = request;
    }
}

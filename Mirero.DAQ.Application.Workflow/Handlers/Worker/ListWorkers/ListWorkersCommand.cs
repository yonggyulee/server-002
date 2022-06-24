using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Worker.ListWorkers;

public class ListWorkersCommand : IRequest<ListWorkersResponse>
{
    public ListWorkersRequest Request { get; private set; }

    public ListWorkersCommand(ListWorkersRequest request)
    {
        Request = request;
    }
}

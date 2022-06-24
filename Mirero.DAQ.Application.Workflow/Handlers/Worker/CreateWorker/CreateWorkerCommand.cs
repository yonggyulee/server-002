using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Worker.CreateWorker;

public class CreateWorkerCommand : IRequest<Domain.Workflow.Protos.V1.Worker>
{
    public CreateWorkerRequest Request { get; set; }

    public CreateWorkerCommand(CreateWorkerRequest request)
    {
        Request = request;
    }
}

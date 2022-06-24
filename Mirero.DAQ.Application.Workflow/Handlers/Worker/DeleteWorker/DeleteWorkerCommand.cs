using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mirero.DAQ.Application.Workflow.Handlers.Worker.DeleteWorker;

public class DeleteWorkerCommand : IRequest<Empty>
{
    public DeleteWorkerRequest Request { get; private set; }

    public DeleteWorkerCommand(DeleteWorkerRequest request)
    {
        Request = request;
    }
}

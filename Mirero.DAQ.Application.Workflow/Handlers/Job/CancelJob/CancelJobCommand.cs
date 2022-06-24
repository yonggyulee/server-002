using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.CancelJob;

public class CancelJobCommand : IRequest<Empty>
{
    public CancelJobRequest Request { get; set; }

    public CancelJobCommand(CancelJobRequest request)
    {
        Request = request;
    }
}
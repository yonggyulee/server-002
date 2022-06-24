using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.CancelBatchJob;

public class CancelBatchJobCommand : IRequest<Empty>
{
    public CancelBatchJobRequest Request { get; set; }

    public CancelBatchJobCommand(CancelBatchJobRequest request)
    {
        Request = request;
    }
}
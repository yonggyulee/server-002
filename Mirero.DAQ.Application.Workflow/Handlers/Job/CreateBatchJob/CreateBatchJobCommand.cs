using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.CreateBatchJob;

public class CreateBatchJobCommand : IRequest<BatchJob>
{
    public CreateBatchJobRequest Request { get; set; }

    public CreateBatchJobCommand(CreateBatchJobRequest request)
    {
        Request = request;
    }
}
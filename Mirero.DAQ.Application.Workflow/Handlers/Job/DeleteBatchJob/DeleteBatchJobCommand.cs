using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.DeleteBatchJob;

public class DeleteBatchJobCommand : IRequest<Empty>
{
    public DeleteBatchJobRequest Request { get; set; }

    public DeleteBatchJobCommand(DeleteBatchJobRequest request)
    {
        Request = request;
    }
}
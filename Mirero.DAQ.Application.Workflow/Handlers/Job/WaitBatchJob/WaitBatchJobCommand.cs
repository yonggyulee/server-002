using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.WaitBatchJob;

public class WaitBatchJobCommand : IRequest<Empty>
{
    public WaitBatchJobRequest Request { get; private set; }
    public IServerStreamWriter<WaitBatchJobResponse> ResponseStream { get; private set; }
    public WaitBatchJobCommand(WaitBatchJobRequest request, IServerStreamWriter<WaitBatchJobResponse> responseStream)
    {
        Request = request;
        ResponseStream = responseStream;
    }
}
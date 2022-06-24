using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.StartJob;

public class StartJobCommand : IRequest<Empty>
{
    public IAsyncStreamReader<StartJobRequest> RequestStream { get; set; }

    public StartJobCommand(IAsyncStreamReader<StartJobRequest> requestStream)
    {
        RequestStream = requestStream;
    }
}
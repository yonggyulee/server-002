using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Common.Protos;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.UploadWorkflowVersion;

public class UploadWorkflowVersionCommand: IRequest<Empty>
{
    public IAsyncStreamReader<IdentifiedStreamBuffer> RequestStream { get; private set; }

    public UploadWorkflowVersionCommand(IAsyncStreamReader<IdentifiedStreamBuffer> requestStream)
    {
        RequestStream = requestStream;
    }
}
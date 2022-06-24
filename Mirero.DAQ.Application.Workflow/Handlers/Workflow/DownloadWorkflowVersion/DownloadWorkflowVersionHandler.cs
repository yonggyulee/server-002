using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Application.Workflow.StreamCreator;
using Mirero.DAQ.Infrastructure.Grpc.Extensions;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.DownloadWorkflowVersion;

public class DownloadWorkflowVersionHandler : IRequestHandler<DownloadWorkflowVersionCommand, Empty>
{
    private readonly WorkflowDownloadStreamCreator _streamCreator;
    public DownloadWorkflowVersionHandler(WorkflowDownloadStreamCreator streamCreator)
    {
        _streamCreator = streamCreator ?? throw new ArgumentNullException(nameof(streamCreator));
    }

    public async Task<Empty> Handle(DownloadWorkflowVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var responseStream = command.ResponseStream;

        await using (var stream = await _streamCreator.ReadStreamAsync(request.WorkflowVersionId, cancellationToken))
        {
             await stream.WriteGrpcServerStreamWriterAsync(responseStream, request.ChunkSize, cancellationToken);
        } 
       
        return new Empty();
    }
}

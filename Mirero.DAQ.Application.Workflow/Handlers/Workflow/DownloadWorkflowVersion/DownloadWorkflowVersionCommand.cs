using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Protos.V1;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.DownloadWorkflowVersion;

public class DownloadWorkflowVersionCommand : IRequest<Empty>
{ 
    public DownloadWorkflowVersionRequest Request { get; private set; }
    public IServerStreamWriter<StreamBuffer> ResponseStream { get; private set; }

    public DownloadWorkflowVersionCommand(DownloadWorkflowVersionRequest request
        , IServerStreamWriter<StreamBuffer> response)
    {
        Request = request;
        ResponseStream = response;
    }
}

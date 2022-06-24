using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Update.Protos.V1;

namespace Mirero.DAQ.Application.Update.Handlers.Rc.DownloadRcStream;

public sealed class DownloadRcSetupVersionCommand : IRequest<Empty>
{
    public DownloadRcSetupVersionRequest Request { get; set; }
    public IServerStreamWriter<StreamBuffer> ResponseStream { get; set; }

    public DownloadRcSetupVersionCommand(DownloadRcSetupVersionRequest request,
        IServerStreamWriter<StreamBuffer> responseStream)
    {
        Request = request;
        ResponseStream = responseStream;
    }
}
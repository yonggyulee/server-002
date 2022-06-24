using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Update.Protos.V1;

namespace Mirero.DAQ.Application.Update.Handlers.Mpp.DownloadMppStream;

public sealed class DownloadMppSetupVersionCommand : IRequest<Empty>
{
    public DownloadMppSetupVersionRequest Request { get; set; }
    public IServerStreamWriter<StreamBuffer> ResponseStream { get; set; }

    public DownloadMppSetupVersionCommand(DownloadMppSetupVersionRequest request, 
        IServerStreamWriter<StreamBuffer> responseStream)
    {
        Request = request;
        ResponseStream = responseStream;
    }
}


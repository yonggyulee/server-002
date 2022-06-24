using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.DownloadGdsStream;

public class DownloadGdsStreamCommand : IRequest
{
    public DownloadGdsStreamRequest Request { get; }
    public IServerStreamWriter<StreamBuffer> ResponseStream { get; }

    public DownloadGdsStreamCommand(DownloadGdsStreamRequest request,
        IServerStreamWriter<StreamBuffer> response)
    {
        Request = request;
        ResponseStream = response;
    }
}
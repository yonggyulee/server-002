using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.DownloadModelVersionStream;

public class DownloadModelVersionStreamCommand : IRequest
{
    public DownloadModelVersionRequest Request { get; set; }
    public IServerStreamWriter<DownloadModelVersionResponse> ResponseStream { get; set; }

    public DownloadModelVersionStreamCommand(DownloadModelVersionRequest request,
        IServerStreamWriter<DownloadModelVersionResponse> responseStream)
    {
        Request = request;
        ResponseStream = responseStream;
    }
}
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Inference.Protos.V1;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.UploadModelVersionStream;

public class UploadModelVersionStreamCommand : IRequest<Empty>
{
    public IAsyncStreamReader<UploadModelVersionRequest> RequestStream { get; set; }

    public UploadModelVersionStreamCommand(IAsyncStreamReader<UploadModelVersionRequest> requestStream)
    {
        RequestStream = requestStream;
    }
}
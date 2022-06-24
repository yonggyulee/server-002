using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Mirero.DAQ.Domain.Gds.Protos.V1;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.UploadGdsStream;

public class UploadGdsStreamCommand : IRequest<Empty>
{
    public IAsyncStreamReader<UploadGdsStreamRequest> RequestStream { get; }

    public UploadGdsStreamCommand(IAsyncStreamReader<UploadGdsStreamRequest> requestStream)
    {
        RequestStream = requestStream;
    }
}
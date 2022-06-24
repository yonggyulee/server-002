using Grpc.Core;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Infrastructure.Grpc.Notifier;
using Mirero.DAQ.Infrastructure.Grpc.StreamCreator;

namespace Mirero.DAQ.Infrastructure.Grpc.Extensions;

public static class StreamReaderExtension
{
    public static async Task ReadGrpcServerStreamReaderAsync(this IAsyncStreamReader<IdentifiedStreamBuffer> streamReader
        , IIdentifiedWriteStreamCreator streamCreator
        , INotifier? notifier = null
        , CancellationToken cancellationToken = default)
    {
        Stream? stream = null;
        var receivedTotalSize = 0L;
        var uploadedTotalSize = 0L;
        try
        {
            await foreach (var current in streamReader.ReadAllAsync(cancellationToken))
            {
                if (stream is null)
                {
                    stream = await streamCreator.CreateStreamAsync(current.Id, cancellationToken);
                    receivedTotalSize = current.TotalSize;
                }

                await stream.WriteAsync(current.Buffer.ToByteArray().AsMemory(0, (int)current.ChunkSize), cancellationToken);
                uploadedTotalSize += current.ChunkSize;

                if (notifier is not null)
                {
                    await notifier.NotifyAsync();
                }
            }
        }
        finally
        {
            if (stream is not null)
            {
                await stream.DisposeAsync();
            }
        }
        
        if (uploadedTotalSize != receivedTotalSize)
        {
            streamCreator.DeleteStreamAsync(cancellationToken);
            throw new Exception("Fail");
        }        
    }
}
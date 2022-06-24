using Google.Protobuf;
using Grpc.Core;
using Mirero.DAQ.Domain.Common.Protos;

namespace Mirero.DAQ.Infrastructure.Grpc.Extensions;

public static class StreamExtension
{
    public static async Task WriteGrpcServerStreamWriterAsync(this Stream stream, IServerStreamWriter<StreamBuffer> streamWriter,
        long chunkSize, CancellationToken cancellationToken)
    {
        var chunkIndex = 0;
        var currentPosition = 0L;
        var buffer = new byte[chunkSize];
        var totalLength = stream.Length;

        while (await stream.ReadAsync(buffer.AsMemory(0, (int)chunkSize), cancellationToken) != 0)
        {
            chunkSize = Math.Min(chunkSize, stream.Length - currentPosition);
            buffer = new byte[chunkSize];
            await streamWriter.WriteAsync(new StreamBuffer
            {
                TotalSize = totalLength,
                ChunkSize = chunkSize,
                ChunkIndex = chunkIndex,
                Buffer = ByteString.CopyFrom(buffer)
            });

            currentPosition = stream.Position;
            chunkIndex++;
        }
        
        if (totalLength != currentPosition)
        {
            throw new InvalidOperationException($"DownLoad 작업 중 파일 크기 {currentPosition} bytes를 전송 하지 못하였습니다(전송된 파일 크기 = {totalLength}");
        }
    }
}
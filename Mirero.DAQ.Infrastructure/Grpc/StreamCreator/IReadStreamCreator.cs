namespace Mirero.DAQ.Infrastructure.Grpc.StreamCreator;

public interface IReadStreamCreator
{
    Task<Stream> ReadStreamAsync(long id, CancellationToken cancellationToken = default);
}
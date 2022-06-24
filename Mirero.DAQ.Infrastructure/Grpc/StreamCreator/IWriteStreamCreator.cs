namespace Mirero.DAQ.Infrastructure.Grpc.StreamCreator;

public interface IWriteStreamCreator
{
    Task<Stream> CreateStreamAsync(long id, CancellationToken cancellationToken = default);
    void DeleteStreamAsync(CancellationToken cancellationToken = default);
}
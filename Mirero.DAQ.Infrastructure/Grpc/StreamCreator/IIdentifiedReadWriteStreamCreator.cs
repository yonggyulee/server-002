namespace Mirero.DAQ.Infrastructure.Grpc.StreamCreator;

public interface IIdentifiedReadWriteStreamCreator : IIdentifiedStreamCreator, IWriteStreamCreator, IReadStreamCreator
{
    
}
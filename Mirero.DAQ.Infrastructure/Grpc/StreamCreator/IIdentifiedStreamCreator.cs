namespace Mirero.DAQ.Infrastructure.Grpc.StreamCreator;

public interface IIdentifiedStreamCreator
{
    long? GetId();
}
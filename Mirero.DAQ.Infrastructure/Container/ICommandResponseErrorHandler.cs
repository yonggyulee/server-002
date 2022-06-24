namespace Mirero.DAQ.Infrastructure.Container;

public interface ICommandResponseErrorHandler
{
    void Handle(string response);
}

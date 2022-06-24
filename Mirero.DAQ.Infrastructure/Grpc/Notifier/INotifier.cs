namespace Mirero.DAQ.Infrastructure.Grpc.Notifier;

public interface INotifier
{
    Task NotifyAsync();
}
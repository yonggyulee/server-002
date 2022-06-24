namespace Mirero.DAQ.Infrastructure.Container;

public interface IContainer
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
    Task RestartAsync(CancellationToken cancellationToken = default);
    Task<ContainerStatus> GetStatusAsync(CancellationToken cancellationToken = default);
}
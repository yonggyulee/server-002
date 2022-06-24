using Mirero.DAQ.Infrastructure.Container.Docker;

namespace Mirero.DAQ.Infrastructure.Container;

public interface IContainerFactory
{
    public Task<IContainer> CreateAsync(Uri uri, string name, ContainerOptionBuilder option, CancellationToken cancellationToken = default);
    public Task<IContainer> GetAsync(Uri uri, string name, CancellationToken cancellationToken = default);
    public Task RemoveAsync(Uri uri, string name, CancellationToken cancellationToken = default);
}
using Microsoft.Extensions.Logging;

namespace Mirero.DAQ.Infrastructure.Locking;

public abstract class LockProviderBase : ILockProvider
{
    public async ValueTask DisposeAsync()
    {
        var isDebugLogLevelEnabled = Logger.IsEnabled(LogLevel.Debug);
        if (isDebugLogLevelEnabled)
            Logger.LogDebug("Disposing LockProvider.");

        await DisposeAsyncCore();
        Dispose(false);
        GC.SuppressFinalize(this);

        if (isDebugLogLevelEnabled)
            Logger.LogDebug("Disposed LockProvider.");
    }

    public abstract Task<ILock?> TryAcquireAsync(string resource, CancellationToken cancellationToken = default);

    public abstract Task<ILock> AcquireAsync(string resource, TimeSpan? timeout = null,
        CancellationToken cancellationToken = default);

    public abstract Task<ILock> AcquireReadLockAsync(string resource, double timeoutSeconds = -1,
        CancellationToken cancellationToken = default);

    public abstract Task<ILock> AcquireWriteLockAsync(string resource, double timeoutSeconds = -1,
        CancellationToken cancellationToken = default);

    public abstract Task ReleaseAsync(ILock @lock, bool isTry, CancellationToken cancellationToken = default);

    public string GenerateLockId<TEntity>(object id)
    {
        return typeof(TEntity).Name + id.ToString();
    }

    public abstract ILogger Logger { get; }

    // 비동기 Dispose와 동기 Dispose 동작이 다를 경우 이 메소드를 오버라이드하여 구현.
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DisposeAsyncCore().Wait();
        }
    }

    public void Dispose()
    {
        var isDebugLogLevelEnabled = Logger.IsEnabled(LogLevel.Debug);
        if (isDebugLogLevelEnabled)
            Logger.LogDebug("Disposing LockProvider.");

        Dispose(true);
        GC.SuppressFinalize(this);

        if (isDebugLogLevelEnabled)
            Logger.LogDebug("Disposed LockProvider.");
    }

    // 비동기 Dispose 동작은 이 메소드를 오버라이드하여 구현.
    protected abstract Task DisposeAsyncCore();
}
using Microsoft.Extensions.Logging;

namespace Mirero.DAQ.Infrastructure.Locking;

public abstract class LockBase : ILock
{
    protected readonly ILockProvider LockProvider;
    protected readonly ILogger Logger;

    protected LockBase(ILockProvider lockProvider, ILogger logger)
    {
        LockProvider = lockProvider;
        Logger = logger;
    }

    public abstract Task ReleaseAsync();

    public abstract ILockKey LockKey { get; }
    public abstract string Resource { get; }
    public abstract DateTime AcquiredTimeUtc { get; }
    public abstract TimeSpan TimeWaitedForLock { get; }

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
        var lockCount = Resource.Split("&").Length;
        if (isDebugLogLevelEnabled)
            Logger.LogDebug("Disposing {LockCount} locks {Resource}", lockCount, Resource);

        Dispose(true);
        GC.SuppressFinalize(this);

        if (isDebugLogLevelEnabled)
            Logger.LogDebug("Disposed {LockCount} locks {Resource}", lockCount, Resource);
    }

    // 비동기 Dispose 동작은 이 메소드를 오버라이드하여 구현.
    protected abstract Task DisposeAsyncCore();
    //{
    //    await ReleaseAsync();
    //}

    public async ValueTask DisposeAsync()
    {
        var isDebugLogLevelEnabled = Logger.IsEnabled(LogLevel.Debug);
        var lockCount = Resource.Split("&").Length;
        if (isDebugLogLevelEnabled)
            Logger.LogDebug("Disposing {LockCount} locks {Resource}", lockCount, Resource);

        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(false);
        GC.SuppressFinalize(this);

        if (isDebugLogLevelEnabled)
            Logger.LogDebug("Disposed {LockCount} locks {Resource}", lockCount, Resource);
    }
}
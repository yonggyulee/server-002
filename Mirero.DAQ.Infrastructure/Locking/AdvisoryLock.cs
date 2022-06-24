using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Mirero.DAQ.Infrastructure.Locking;

public sealed class AdvisoryLock : LockBase
{
    private bool _isReleased;
    public bool IsShared { get; }
    private readonly object _lock = new();

    public AdvisoryLock(string resource, ILockKey lockKey, ILockProvider lockProvider, ILogger logger, bool isShared) :
        base(lockProvider, logger)
    {
        Resource = resource;
        LockKey = lockKey;
        IsShared = isShared;
    }

    protected override async Task DisposeAsyncCore()
    {
        try
        {
            await ReleaseAsync();
        }
        catch (Exception ex)
        {
            if (Logger.IsEnabled(LogLevel.Error))
                Logger.LogError(ex, "Unable to release lock {Resource}", Resource);
        }
    }

    public override Task ReleaseAsync() => ReleaseAsync(false);
    public override ILockKey LockKey { get; }
    public override string Resource { get; }
    public override DateTime AcquiredTimeUtc { get; }
    public override TimeSpan TimeWaitedForLock { get; }

    public Task ReleaseAsync(bool isTry, CancellationToken cancellationToken = default)
    {
        if (_isReleased)
            return Task.CompletedTask;

        lock (_lock)
        {
            if (_isReleased)
                return Task.CompletedTask;

            _isReleased = true;

            if (Logger.IsEnabled(LogLevel.Debug))
                Logger.LogDebug("Releasing lock {Resource}", Resource);

            return LockProvider.ReleaseAsync(this, isTry, cancellationToken);
        }
    }
}
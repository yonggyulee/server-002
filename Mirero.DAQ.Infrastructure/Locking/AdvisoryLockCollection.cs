using Microsoft.Extensions.Logging;

namespace Mirero.DAQ.Infrastructure.Locking;

public sealed class AdvisoryLockCollection : LockBase
{
    private readonly List<ILock> _locks = new();
    private bool _isReleased;
    private readonly object _lock = new();

    public AdvisoryLockCollection(IEnumerable<ILock> locks, ILockProvider lockProvider, ILogger logger) : base(lockProvider, logger)
    {
        _locks.AddRange(locks);
        Resource = string.Join("&", _locks.Select(l => l.Resource));
        LockKey = new AdvisoryLockKey(Resource, allowHashing: true);
    }

    //public AdvisoryLockCollection(IEnumerable<AdvisoryLockCollection> collections, ILockProvider lockProvider,
    //    ILogger logger) : base(lockProvider, logger)
    //{
    //    Resource = string.Join("&", collections.Select(c =>
    //    {
    //        _locks.AddRange(c._locks);
    //        return c.Resource;
    //    }));
    //    LockKey = new AdvisoryLockKey(Resource, allowHashing: true);
    //}

    public override Task ReleaseAsync()
    {
        if (_isReleased)
            return Task.CompletedTask;

        lock (_lock)
        {
            if (_isReleased)
                return Task.CompletedTask;

            _isReleased = true;

            if (Logger.IsEnabled(LogLevel.Debug))
                Logger.LogDebug("Releasing {LockCount} locks {Resource}", _locks.Count, Resource);

            //Task.Run(() => _locks.Select(l => l.ReleaseAsync()));

            return Task.Run(async () =>
            {
                foreach (var @lock in _locks) await @lock.DisposeAsync();
            });
        }
    }

    public override ILockKey LockKey { get; }
    public override string Resource { get; }
    public override DateTime AcquiredTimeUtc { get; }
    public override TimeSpan TimeWaitedForLock { get; }

    protected override async Task DisposeAsyncCore()
    {
        try
        {
            await ReleaseAsync();
        }
        catch (Exception ex)
        {
            if (Logger.IsEnabled(LogLevel.Error))
                Logger.LogError(ex, "Unable to release {LockCount} locks {Resource}", _locks.Count, Resource);
        }
    }
}
namespace Mirero.DAQ.Infrastructure.Locking;

public interface ILock : IDisposable, IAsyncDisposable
{
    Task ReleaseAsync();
    ILockKey LockKey { get; }
    string Resource { get; }
    DateTime AcquiredTimeUtc { get; }
    TimeSpan TimeWaitedForLock { get; }
}
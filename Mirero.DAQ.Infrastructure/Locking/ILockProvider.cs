using Microsoft.Extensions.Logging;

namespace Mirero.DAQ.Infrastructure.Locking;

public interface ILockProvider : IDisposable, IAsyncDisposable
{
    Task<ILock?> TryAcquireAsync(string resource, CancellationToken cancellationToken = default);
    Task<ILock> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default);

    Task<ILock> AcquireReadLockAsync(string resource, double timeoutSeconds = -1, CancellationToken cancellationToken = default);
    Task<ILock> AcquireWriteLockAsync(string resource, double timeoutSeconds = -1, CancellationToken cancellationToken = default);
    
    Task ReleaseAsync(ILock @lock, bool isTry, CancellationToken cancellationToken = default);

    string GenerateLockId<TEntity>(object id);

    ILogger Logger { get; }
}

public static class LockProviderExtension
{
    public static async Task<ILock> TryAcquireAsync(this ILockProvider lockProvider, IEnumerable<string> resources,
        double timeoutSeconds = -1, CancellationToken cancellationToken = default)
    {
        if (resources == null)
            throw new ArgumentNullException(nameof(resources));
        var resourceList = resources.Distinct().ToArray();
        if (resourceList.Length == 0)
            throw new NotImplementedException($"{nameof(resourceList)} is empty.");

        var logger = lockProvider.Logger;

        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace("Acquiring {LockCount} locks {Resource}", resourceList.Length, resourceList);

        var timeout = timeoutSeconds < 0 ? TimeSpan.FromSeconds(timeoutSeconds) : (TimeSpan?)null;

        var locks = await Task.WhenAll(
            resourceList.Select(r => lockProvider.AcquireAsync(r, timeout, cancellationToken)));

        //var acquiredLocks = locks.Where(l => l != null).ToArray();
        //var unacquiredResources = resourceList.Except(locks.Select(l => l?.Resource)).ToArray();
        //if (unacquiredResources.Length > 0)
        //{
        //    if (logger.IsEnabled(LogLevel.Trace))
        //        logger.LogTrace("Unable to acquire all {LockCount} locks {Resource} releasing acquired locks", unacquiredResources.Length, unacquiredResources);

        //    await Task.WhenAll(acquiredLocks.Select(l => l.ReleaseAsync()));
        //    return null;
        //}

        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace("Acquired {LockCount} locks {Resource}", resourceList.Length, resourceList);

        return new AdvisoryLockCollection(locks, lockProvider, logger);
    }

    public static async Task<ILock> AcquireAsync(this ILockProvider lockProvider, IEnumerable<string> resources,
        double timeoutSeconds = -1, CancellationToken cancellationToken = default)
    {
        if (resources == null)
            throw new ArgumentNullException(nameof(resources));
        var resourceList = resources.Distinct().ToArray();
        if (resourceList.Length == 0)
            throw new NotImplementedException($"{nameof(resourceList)} is empty.");

        var logger = lockProvider.Logger;

        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace("Acquiring {LockCount} locks {Resource}", resourceList.Length, resourceList);

        var timeout = timeoutSeconds < 0 ? TimeSpan.FromSeconds(timeoutSeconds) : (TimeSpan?) null;

        var locks = await Task.Run(
            () => resourceList.Select(r => lockProvider.AcquireAsync(r,  timeout, cancellationToken).Result),
            cancellationToken);

        //var acquiredLocks = locks.Where(l => l != null).ToArray();
        //var unacquiredResources = resourceList.Except(locks.Select(l => l?.Resource)).ToArray();
        //if (unacquiredResources.Length > 0)
        //{
        //    if (logger.IsEnabled(LogLevel.Trace))
        //        logger.LogTrace("Unable to acquire all {LockCount} locks {Resource} releasing acquired locks", unacquiredResources.Length, unacquiredResources);

        //    await Task.WhenAll(acquiredLocks.Select(l => l.ReleaseAsync()));
        //    return null;
        //}

        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace("Acquired {LockCount} locks {Resource}", resourceList.Length, resourceList);

        return new AdvisoryLockCollection(locks, lockProvider, logger);
    }

    public static async Task<ILock> AcquireReadLockAsync(this ILockProvider lockProvider, IEnumerable<string> resources,
        double timeoutSeconds = -1, CancellationToken cancellationToken = default)
    {
        if (resources == null)
            throw new ArgumentNullException(nameof(resources));
        var resourceList = resources.Distinct().ToArray();
        if (resourceList.Length == 0)
            throw new NotImplementedException($"{nameof(resourceList)} is empty.");

        var logger = lockProvider.Logger;

        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace("Acquiring {LockCount} locks {Resource}", resourceList.Length, resourceList);
        
        var locks = await Task.Run(
            () => resourceList.Select(r => lockProvider.AcquireReadLockAsync(r, timeoutSeconds, cancellationToken).Result),
            cancellationToken);

        //var acquiredLocks = locks.Where(l => l != null).ToArray();
        //var unacquiredResources = resourceList.Except(locks.Select(l => l?.Resource)).ToArray();
        //if (unacquiredResources.Length > 0)
        //{
        //    if (logger.IsEnabled(LogLevel.Trace))
        //        logger.LogTrace("Unable to acquire all {LockCount} locks {Resource} releasing acquired locks", unacquiredResources.Length, unacquiredResources);

        //    await Task.WhenAll(acquiredLocks.Select(l => l.ReleaseAsync()));
        //    return null;
        //}

        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace("Acquired {LockCount} locks {Resource}", resourceList.Length, resourceList);

        return new AdvisoryLockCollection(locks, lockProvider, logger);
    }

    public static async Task<ILock> AcquireWriteLockAsync(this ILockProvider lockProvider,
        IEnumerable<string> resources, double timeoutSeconds = -1, CancellationToken cancellationToken = default)
    {
        if (resources == null)
            throw new ArgumentNullException(nameof(resources));
        var resourceList = resources.Distinct().ToArray();
        if (resourceList.Length == 0)
            throw new NotImplementedException($"{nameof(resourceList)} is empty.");

        var logger = lockProvider.Logger;

        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace("Acquiring {LockCount} locks {Resource}", resourceList.Length, resourceList);

        var locks = await Task.Run(
            () => resourceList.Select(r => lockProvider.AcquireWriteLockAsync(r, timeoutSeconds, cancellationToken).Result),
            cancellationToken);

        //var acquiredLocks = locks.Where(l => l != null).ToArray();
        //var unacquiredResources = resourceList.Except(locks.Select(l => l?.Resource)).ToArray();
        //if (unacquiredResources.Length > 0)
        //{
        //    if (logger.IsEnabled(LogLevel.Trace))
        //        logger.LogTrace("Unable to acquire all {LockCount} locks {Resource} releasing acquired locks", unacquiredResources.Length, unacquiredResources);

        //    await Task.WhenAll(acquiredLocks.Select(l => l.ReleaseAsync()));
        //    return null;
        //}

        if (logger.IsEnabled(LogLevel.Trace))
            logger.LogTrace("Acquired {LockCount} locks {Resource}", resourceList.Length, resourceList);

        return new AdvisoryLockCollection(locks, lockProvider, logger);
    }

    public static async Task<T> ThrowTimeoutIfNull<T>(this Task<T?> task, string? resource = null) where T : class =>
        await task ?? throw new TimeoutException($"Timeout exceeded when trying to acquire the lock.\'{resource ?? string.Empty}\'");
}
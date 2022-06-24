using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace Mirero.DAQ.Infrastructure.Locking;

//public interface ILockMemory : IDisposable, IAsyncDisposable
//{
//    public void AddLock(ILock @lock);
//}


//public class LockMemory : ILockMemory
//{
//    private readonly ILogger _logger;
//    private readonly IList<ILock> _locks = new List<ILock>();

//    public LockMemory(ILogger<LockMemory> logger)
//    {
//        _logger = logger;
//    }

//    public void AddLock(ILock @lock)
//    {
//        _locks.Add(@lock);
//    }

//    public void Dispose()
//    {
//        var isDebugLogLevelEnabled = _logger.IsEnabled(LogLevel.Debug);
//        if (isDebugLogLevelEnabled)
//            _logger.LogDebug("Disposing GlobalLockCollection.");
        
//        DisposeAsync().GetAwaiter().GetResult();
//        GC.SuppressFinalize(this);

//        if (isDebugLogLevelEnabled)
//            _logger.LogDebug("Disposed GlobalLockCollection.");
//    }

//    public async ValueTask DisposeAsync()
//    {
//        var isDebugLogLevelEnabled = _logger.IsEnabled(LogLevel.Debug);
//        if (isDebugLogLevelEnabled)
//            _logger.LogDebug("Disposing GlobalLockCollection.");
//        foreach (var @lock in _locks)
//        {
//            await @lock.DisposeAsync();
//        }
//        GC.SuppressFinalize(this);

//        if (isDebugLogLevelEnabled)
//            _logger.LogDebug("Disposed GlobalLockCollection.");
//    }

//    ~LockMemory()
//    {
//        Dispose();
//    }
//}
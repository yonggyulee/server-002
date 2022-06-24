using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Mirero.DAQ.Infrastructure.Caching;

public class CacheClient<T> : IDisposable where T : ICloneable
{
    private readonly ILogger _logger;

    private readonly ConcurrentDictionary<string, CacheEntry<T>> _memory;
    private readonly int? _maxCount;
    private DateTime _lastMaintenance;
    private bool _shouldClone;

    public CacheClient(ILogger logger, int maxCount, bool shouldClone)
    {
        _logger = logger;

        _memory = new ConcurrentDictionary<string, CacheEntry<T>>();
        _maxCount = maxCount;
        _shouldClone = shouldClone;
    }

    public int Count => _memory.Count;

    public bool Add(string key, T value, TimeSpan? expiresIn = null)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");

        var expiresAt = expiresIn.HasValue ? GetExpiresAt(DateTime.UtcNow, expiresIn.Value) : DateTime.MaxValue;

        var entry = new CacheEntry<T>(value, expiresAt, _shouldClone);
        
        return SetInternal(key, entry, true);
    }

    public bool Set(string key, T value, TimeSpan? expiresIn = null)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");

        var expiresAt = expiresIn.HasValue ? GetExpiresAt(DateTime.UtcNow, expiresIn.Value) : DateTime.MaxValue;

        var entry = new CacheEntry<T>(value, expiresAt, _shouldClone);

        return SetInternal(key, entry);
    }

    public bool Remove(string key, bool isExpired = false)
    {
        if (isExpired)
            _logger.LogDebug("Removing expired cache entry {Key}", key);

        return _memory.TryRemove(key, out _);
    }

    public int RemoveAll(IEnumerable<string>? keys = null)
    {
        if (keys == null)
        {
            var count = _memory.Count;
            _memory.Clear();
            return count;
        }

        var removed = 0;
        foreach (var key in keys)
        {
            if (string.IsNullOrEmpty(key))
                continue;

            if (_logger.IsEnabled(LogLevel.Trace)) _logger.LogTrace("RemoveAllAsync: Removing key: {Key}", key);
            if (_memory.TryRemove(key, out _))
                removed++;
        }

        return removed;
    }

    public T? Get(string key)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty");

        if (!_memory.TryGetValue(key, out var cacheEntry)) return default;

        if (cacheEntry.ExpiresAt >= DateTime.UtcNow) return cacheEntry.Value;
        Remove(key, true);
        return default;
    }

    public IDictionary<string, T> GetAll(IEnumerable<string>? keys = null)
    {
        if (keys == null)
        {
            return _memory.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value);
        }

        var map = new Dictionary<string, T>();

        foreach (var key in keys)
        {
            var value = this.Get(key);
            if (value == null) continue;
            map[key] = value;
        }

        return map;
    }

    private bool SetInternal(string key, CacheEntry<T> entry, bool addOnly = false)
    {
        if (entry.ExpiresAt < DateTime.UtcNow) return false;

        if (addOnly)
        {
            if (!_memory.TryAdd(key, entry))
            {
                if (!_memory.TryGetValue(key, out var existingEntry) || existingEntry.ExpiresAt < DateTime.UtcNow)
                {
                    _memory.AddOrUpdate(key, entry, (k, cacheEntry) => entry);
                }
                else
                {
                    return false;
                }
            }

            if (_logger.IsEnabled(LogLevel.Trace)) _logger.LogTrace("Added cache key: {Key}", key);
        }
        else
        {
            _memory.AddOrUpdate(key, entry, (k, cacheEntry) => entry);
            if (_logger.IsEnabled(LogLevel.Trace)) _logger.LogTrace("Set cache key: {Key}", key);
        }

        StartMaintenance(true);

        return true;
    }

    private void StartMaintenance(bool compactImmediately = false)
    {
        var now = DateTime.UtcNow;
        if (compactImmediately)
            Compact();

        if (TimeSpan.FromMilliseconds(100) < now - _lastMaintenance)
        {
            _lastMaintenance = now;
            _ = Task.Run(DoMaintenance);
        }
    }

    private void DoMaintenance()
    {
        _logger.LogTrace("DoMaintenance");

        var utcNow = DateTime.UtcNow;

        try
        {
            foreach (var (key, value) in _memory.ToArray())
            {
                var expiresAt = value.ExpiresAt;
                if (expiresAt <= utcNow)
                    Remove(key, true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error trying to find expired cache items");
        }

        Compact();
    }

    private void Compact()
    {
        if (!IsFullMemory)
            return;

        (string? Key, long LastAccessTicks) oldest = (null, long.MaxValue);
        foreach (var (key, value) in _memory)
        {
            if (value.LastAccessTicks <= oldest.LastAccessTicks)
                oldest = (Key: key, value.LastAccessTicks);
        }

        _logger.LogDebug("Removing cache entry {Key} due to cache exceeding max item count limit.", oldest);
        if (oldest.Key != null) _memory.TryRemove(oldest.Key, out var cacheEntry);
    }

    private DateTime GetExpiresAt(DateTime date, TimeSpan value)
    {
        if (date.Ticks + value.Ticks < DateTime.MinValue.Ticks)
            return DateTime.MinValue;

        if (date.Ticks + value.Ticks > DateTime.MaxValue.Ticks)
            return DateTime.MaxValue;

        return date.Add(value);
    }

    private bool IsFullMemory => _maxCount.HasValue && _memory.Count >= _maxCount;

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _memory.Clear();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
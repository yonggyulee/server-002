using System.Reflection;

namespace Mirero.DAQ.Infrastructure.Caching;

public class CacheEntry<T> where T : ICloneable
{
    private T _cacheValue;
    private readonly bool _shouldClone;

    public CacheEntry(T value, DateTime expiresAt, bool shouldClone = true)
    {
        _shouldClone = shouldClone;
        Value = value;
        ExpiresAt = expiresAt;
        LastModifiedTicks = DateTime.UtcNow.Ticks;
    }
    
    public DateTime ExpiresAt { get; set; }
    public long LastAccessTicks { get; private set; }
    public long LastModifiedTicks { get; private set; }

    public T Value
    {
        get
        {
            LastAccessTicks = DateTime.UtcNow.Ticks;
            return _shouldClone ? (T)_cacheValue.Clone() : _cacheValue;
        }
        set
        {
            _cacheValue = _shouldClone ? (T)value.Clone() : value;
            LastAccessTicks = DateTime.UtcNow.Ticks;
            LastModifiedTicks = DateTime.UtcNow.Ticks;
        }
    }
}
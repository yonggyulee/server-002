using Microsoft.Extensions.Logging;

namespace Mirero.DAQ.Infrastructure.Caching;

public class CacheItemProvider<T> : ICacheItemProvider<T, T> where T : ICloneable
{
    private readonly ILogger<CacheItemProvider<T>> _logger;
    private readonly CacheClient<T> _cacheClient;

    public CacheItemProvider(ILogger<CacheItemProvider<T>> logger, int maxCount, bool shouldClone)
    {
        _cacheClient = new CacheClient<T>(logger, maxCount, shouldClone);
        _logger = logger;
    }

    public T Get(string key)
    {
        var item = _cacheClient.Get(key);
        if (item == null) throw new NullReferenceException($"Value({key}) is null.");
        return item;
    }

    public T? GetOrDefault(string key)
    {
        return _cacheClient.Get(key);
    }

    public IDictionary<string, T> GetAll(IEnumerable<string>? keys = null)
    {
        return _cacheClient.GetAll(keys);
    }

    public bool Add(string key, T value, TimeSpan? expiresIn = null)
    {
        return _cacheClient.Add(key, value, expiresIn);
    }

    public bool Set(string key, T value, TimeSpan? expiresIn = null)
    {
        return _cacheClient.Set(key, value, expiresIn);
    }

    public bool Remove(string key)
    {
        return _cacheClient.Remove(key);
    }

    public int RemoveAll(IEnumerable<string>? keys = null)
    {
        return _cacheClient.RemoveAll(keys);
    }
}
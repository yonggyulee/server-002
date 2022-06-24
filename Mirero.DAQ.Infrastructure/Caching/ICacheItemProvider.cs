namespace Mirero.DAQ.Infrastructure.Caching;

public interface ICacheItemProvider<in TValue, T>
{
    T Get(string key);
    T? GetOrDefault(string key);
    IDictionary<string, T> GetAll(IEnumerable<string>? keys = null);
    bool Add(string key, TValue value, TimeSpan? expiresIn = null);
    bool Set(string key, TValue value, TimeSpan? expiresIn = null);
    bool Remove(string key);
    int RemoveAll(IEnumerable<string>? keys = null);
}
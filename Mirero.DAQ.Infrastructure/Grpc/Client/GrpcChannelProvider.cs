using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Caching;

namespace Mirero.DAQ.Infrastructure.Grpc.Client;

public class GrpcChannelProvider : ICacheItemProvider<string, GrpcChannelData>
{
    private readonly ILogger<GrpcChannelProvider> _logger;
    private readonly GrpcChannelCacheClient _cacheClient;
    private readonly TimeSpan? _defaultExpiresTime;

    public GrpcChannelProvider(ILogger<GrpcChannelProvider> logger, IConfiguration configuration)
    {
        _cacheClient = new GrpcChannelCacheClient(logger, configuration.GetValue<int>("Inference:Cache:MaxCount"));
        // TODO : 추후 TimeSpan 문자열 파싱 수정 필요.
        _defaultExpiresTime = TimeSpan.Parse(configuration.GetValue<string>("Inference:Cache:ExpiresTime"));
        _logger = logger;
    }
    
    public GrpcChannelData Get(string key)
    {
        var channel = _cacheClient.Get(key);
        if (channel == null)
            throw new NullReferenceException($"Value({key}) is null.");
        return channel;
    }

    public GrpcChannelData? GetOrDefault(string key)
    {
        var channel = _cacheClient.Get(key);
        return channel;
    }

    public IDictionary<string, GrpcChannelData> GetAll(IEnumerable<string>? keys = null)
    {
        return _cacheClient.GetAll(keys);
    }

    public bool Add(string key, string value, TimeSpan? expiresIn = null)
    {
        return _cacheClient.Add(key, value, expiresIn ?? _defaultExpiresTime);
    }

    public bool Set(string key, string value, TimeSpan? expiresIn = null)
    {
        throw new NotImplementedException();
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
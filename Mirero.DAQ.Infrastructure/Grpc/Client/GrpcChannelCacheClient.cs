using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Caching;

namespace Mirero.DAQ.Infrastructure.Grpc.Client;

public class GrpcChannelCacheClient : CacheClient<GrpcChannelData>
{
    public GrpcChannelCacheClient(ILogger logger, int maxCount, bool shouldClone = false) : base(logger, maxCount,
        shouldClone)
    {
    }

    public bool Add(string key, string address, TimeSpan? expiresIn = null)
    {
        return base.Set(key, new GrpcChannelData(address), expiresIn);
    }

    public bool Remove(string key)
    {
        return base.Remove(key);
    }
}
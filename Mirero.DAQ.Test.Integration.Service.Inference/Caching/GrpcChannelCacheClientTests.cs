using Grpc.Net.Client;
using Microsoft.Extensions.Logging.Abstractions;
using Mirero.DAQ.Infrastructure.Grpc.Client;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Inference.Caching;

public class GrpcChannelCacheClientTests
{
    private static readonly int _maxCount = 100;

    private static GrpcChannelCacheClient? GetCacheClient()
    {
        return new GrpcChannelCacheClient(NullLogger<GrpcChannelCacheClientTests>.Instance, maxCount: _maxCount);
    }

    [Fact]
    public void CanGetAll()
    {
        var cache = GetCacheClient();
        if (cache == null)
            return;

        using (cache)
        {
            cache.RemoveAll();

            Assert.True(cache.Set("test1", new GrpcChannelData("http://192.168.1.1:5001")));
            Assert.True(cache.Set("test2", new GrpcChannelData("http://192.168.1.2:5002")));
            Assert.True(cache.Set("test3", new GrpcChannelData("http://192.168.1.3:5003")));
            var result = cache.GetAll(new[] { "test1", "test2", "test3", "test4" });
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.NotNull(result["test1"]);
            Assert.Equal("http://192.168.1.2:5002", result["test2"].Address);
            Assert.Equal("http://192.168.1.3:5003", result["test3"].Address);
            Assert.False(result.ContainsKey("test4"));
        }
    }

    [Fact]
    public void CanSetAndGet()
    {
        var cache = GetCacheClient();
        if (cache == null)
            return;

        using (cache)
        {
            cache.RemoveAll();

            Assert.Null(cache.Get("donkey"));

            cache.Set("test", new GrpcChannelData("http://192.168.1.1:5001"));
            var result = cache.Get("test");
            Assert.NotNull(result);
            Assert.Equal("http://192.168.1.1:5001", result?.Address);

            Assert.False(cache.Add("test", new GrpcChannelData("http://192.168.1.2:1001")));
            result = cache.Get("test");
            Assert.NotNull(result);
            Assert.Equal("http://192.168.1.1:5001", result?.Address);

            Assert.True(cache.Remove("test"));
            Assert.Null(cache.Get("test"));

            Assert.True(cache.Add("test", new GrpcChannelData("http://192.168.1.3:5003")));
            result = cache.Get("test");
            Assert.NotNull(result);
            Assert.Equal("http://192.168.1.3:5003", result?.Address);
        }
    }

    [Fact]
    public async Task CanAddConcurrentlyAsync()
    {
        var cache = GetCacheClient();
        if (cache == null)
            return;

        using (cache)
        {
            cache.RemoveAll();

            var cacheKey = Guid.NewGuid().ToString("N")[10..];
            long adds = 0;
            await Task.WhenAll(Enumerable.Range(1, 5).Select(i => Task.Run(() => {
                if (cache.Add(cacheKey, new GrpcChannelData("http://192.168.1.1:" + (5000 + i)), TimeSpan.FromMinutes(1)))
                    Interlocked.Increment(ref adds);
            })));

            Assert.Equal(1, adds);
        }
    }

    [Fact]
    public async Task CanSetExpirationAsync()
    {
        var cache = GetCacheClient();
        if (cache == null)
            return;

        using (cache)
        {
            cache.RemoveAll();

            var expiresIn = 300;
            var success = cache.Set("test", new GrpcChannelData("http://192.168.1.1:5001"),
                TimeSpan.FromMilliseconds(expiresIn));
            Assert.True(success);
            expiresIn += 100;
            success = cache.Set("test2", new GrpcChannelData("http://192.168.1.2:5002"),
                TimeSpan.FromMilliseconds(expiresIn));
            Assert.True(success);
            var result = cache.Get("test");
            Assert.NotNull(result);
            Assert.Equal("http://192.168.1.1:5001", result?.Address);

            await Task.Delay(500);
            Assert.Null(cache.Get("test"));
            Assert.Null(cache.Get("test2"));
        }
    }

    [Fact]
    public void CanSetMinMaxExpiration()
    {
        var cache = GetCacheClient();
        if (cache == null)
            return;

        using (cache)
        {
            cache.RemoveAll();

            var now = DateTime.UtcNow;

            var expires = DateTime.MaxValue - now.AddDays(1);
            Assert.True(cache.Set("test1", new GrpcChannelData("http://192.168.1.1:5001"), expires));
            Assert.False(cache.Set("test2", new GrpcChannelData("http://192.168.1.1:5002"),
                DateTime.MinValue.Subtract(DateTime.UtcNow)));
            Assert.True(cache.Set("test3", new GrpcChannelData("http://192.168.1.1:5003"),
                DateTime.MaxValue.Subtract(DateTime.UtcNow)));
            Assert.True(cache.Set("test4", new GrpcChannelData("http://192.168.1.1:5004"),
                DateTime.MaxValue - now.AddDays(-1)));

            var result1 = cache.Get("test1");
            Assert.NotNull(result1);
            Assert.Equal("http://192.168.1.1:5001", result1?.Address);

            Assert.Null(cache.Get("test2"));

            var result3 = cache.Get("test3");
            Assert.NotNull(result3);
            Assert.Equal("http://192.168.1.1:5003", result3?.Address);

            var result4 = cache.Get("test4");
            Assert.NotNull(result4);
            Assert.Equal("http://192.168.1.1:5004", result4?.Address);
        }
    }
}
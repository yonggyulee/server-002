using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Mirero.DAQ.Infrastructure.Caching;
using Xunit;

namespace Mirero.DAQ.Test.Integration.Service.Inference.Caching;

public class CacheClientTests
{
    private static readonly int _maxCount = 100;

    private static CacheClient<T>? GetCacheClient<T>() where T : ICloneable
    {
        return new CacheClient<T>(NullLogger<CacheClientTests>.Instance, maxCount: _maxCount, true);
    }

    [Fact]
    public void CanGetAll()
    {
        var cache = GetCacheClient<SimpleModel>();
        if (cache == null)
            return;

        using (cache)
        {
            cache.RemoveAll();

            cache.Set("test1", new SimpleModel { Data1 = "data 1", Data2 = 1 });
            cache.Set("test2", new SimpleModel { Data1 = "data 2", Data2 = 2 });
            cache.Set("test3", new SimpleModel { Data1 = "data 3", Data2 = 3 });
            var result = cache.GetAll(new[] { "test1", "test2", "test3", "test4" });
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.NotNull(result["test1"]);
            Assert.Equal("data 2", result["test2"].Data1);
            Assert.Equal(3, result["test3"].Data2);
            Assert.False(result.ContainsKey("test4"));
        }
    }

    [Fact]
    public void CanSetAndGet()
    {
        var cache = GetCacheClient<SimpleModel>();
        if (cache == null)
            return;

        using (cache)
        {
            cache.RemoveAll();
            
            Assert.Null(cache.Get("donkey"));

            cache.Set("test", new SimpleModel { Data1 = "data 1", Data2 = 1 });
            var model = cache.Get("test");
            Assert.NotNull(model);
            Assert.Equal("data 1", model?.Data1);

            Assert.False(cache.Add("test", new SimpleModel { Data1 = "data 2", Data2 = 2 }));
            model = cache.Get("test");
            Assert.NotNull(model);
            Assert.Equal("data 1", model?.Data1);

            Assert.True(cache.Remove("test"));
            Assert.Null(cache.Get("test"));

            Assert.True(cache.Add("test", new SimpleModel { Data1 = "data 3", Data2 = 3 }));
            model = cache.Get("test");
            Assert.NotNull(model);
            Assert.Equal("data 3", model?.Data1);
        }
    }

    [Fact]
    public async Task CanAddConcurrentlyAsync()
    {
        var cache = GetCacheClient<SimpleModel>();
        if (cache == null)
            return;

        using (cache)
        {
            cache.RemoveAll();

            var cacheKey = Guid.NewGuid().ToString("N")[10..];
            long adds = 0;
            await Task.WhenAll(Enumerable.Range(1, 5).Select(i => Task.Run(() => {
                if (cache.Add(cacheKey, new SimpleModel { Data1 = "data " + i, Data2 = i }, TimeSpan.FromMinutes(1)))
                    Interlocked.Increment(ref adds);
            })));

            Assert.Equal(1, adds);
        }
    }

    [Fact]
    public async Task CanSetExpirationAsync()
    {
        var cache = GetCacheClient<SimpleModel>();
        if (cache == null)
            return;

        using (cache)
        {
            cache.RemoveAll();

            var expiresIn = 300;
            var success = cache.Set("test", new SimpleModel {Data1 = "data 1", Data2 = 1},
                TimeSpan.FromMilliseconds(expiresIn));
            Assert.True(success);
            expiresIn += 100;
            success = cache.Set("test2", new SimpleModel {Data1 = "data 1", Data2 = 1},
                TimeSpan.FromMilliseconds(expiresIn));
            Assert.True(success);
            var model = cache.Get("test");
            Assert.NotNull(model);
            Assert.Equal("data 1", model?.Data1);

            await Task.Delay(500);
            Assert.Null(cache.Get("test"));
            Assert.Null(cache.Get("test2"));
        }
    }

    [Fact]
    public void CanSetMinMaxExpiration()
    {
        var cache = GetCacheClient<SimpleModel>();
        if (cache == null)
            return;

        using (cache)
        {
            cache.RemoveAll();
            
            var now = DateTime.UtcNow;

            var expires = DateTime.MaxValue - now.AddDays(1);
            Assert.True(cache.Set("test1", new SimpleModel {Data1 = "data 1", Data2 = 1}, expires));
            Assert.False(cache.Set("test2", new SimpleModel {Data1 = "data 2", Data2 = 2},
                DateTime.MinValue.Subtract(DateTime.UtcNow)));
            Assert.True(cache.Set("test3", new SimpleModel {Data1 = "data 3", Data2 = 3},
                DateTime.MaxValue.Subtract(DateTime.UtcNow)));
            Assert.True(cache.Set("test4", new SimpleModel {Data1 = "data 4", Data2 = 4},
                DateTime.MaxValue - now.AddDays(-1)));

            var model1 = cache.Get("test1");
            Assert.NotNull(model1);
            Assert.Equal("data 1", model1?.Data1);

            Assert.Null(cache.Get("test2"));

            var model3 = cache.Get("test3");
            Assert.NotNull(model3);
            Assert.Equal("data 3", model3?.Data1);

            var model4 = cache.Get("test4");
            Assert.NotNull(model4);
            Assert.Equal("data 4", model4?.Data1);
        }
    }

    public class SimpleModel : ICloneable
    {
        public string Data1 { get; set; }
        public int Data2 { get; set; }

        public object Clone()
        {
            return new SimpleModel { Data1 = this.Data1, Data2 = this.Data2};
        }
    }
}
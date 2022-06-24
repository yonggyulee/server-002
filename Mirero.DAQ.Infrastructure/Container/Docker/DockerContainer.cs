using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Mirero.DAQ.Infrastructure.Container.Docker;

public class DockerContainer : IContainer
{
    private readonly Uri _uri;
    private readonly string _name;
    private readonly HttpClient _httpClient;
    private readonly ICommandResponseErrorHandler _responseErrorHandler;

    public DockerContainer(Uri uri, string name, HttpClient httpClient, ICommandResponseErrorHandler responseErrorHandler)
    {
        _uri = uri ?? throw new ArgumentNullException(nameof(uri));
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _responseErrorHandler = responseErrorHandler ?? throw new ArgumentNullException(nameof(responseErrorHandler));
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var httpContent = new StringContent(string.Empty)
            { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };

        var response = await _httpClient.PostAsync(new Uri(_uri, $"containers/{_name}/start"),
            httpContent, cancellationToken);

        if (!response.IsSuccessStatusCode)
            _responseErrorHandler.Handle(await response.Content.ReadAsStringAsync(cancellationToken));
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        var httpContent = new StringContent(string.Empty)
            { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };

        var response = await _httpClient.PostAsync(new Uri(_uri, $"containers/{_name}/stop"),
            httpContent, cancellationToken);

        if (!response.IsSuccessStatusCode)
            _responseErrorHandler.Handle(await response.Content.ReadAsStringAsync(cancellationToken));
    }

    public async Task RestartAsync(CancellationToken cancellationToken = default)
    {
        var httpContent = new StringContent(string.Empty)
            { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };

        var response = await _httpClient.PostAsync(new Uri(_uri, $"containers/{_name}/restart"),
            httpContent, cancellationToken);

        if (!response.IsSuccessStatusCode)
            _responseErrorHandler.Handle(await response.Content.ReadAsStringAsync(cancellationToken));
    }

    public async Task<ContainerStatus> GetStatusAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(new Uri(_uri, $"containers/{_name}/stats?stream=false"), cancellationToken);
        var responseBody = JObject.Parse(await response.Content.ReadAsStringAsync(cancellationToken));

        #region Memory

        var memoryStatsUsage = responseBody.GetValue("memory_stats")!.SelectToken("usage");
        var cacheMemory = responseBody.GetValue("memory_stats")!.SelectToken("stats")!.SelectToken("cache")!.ToObject<long>();
        var usedMemory = memoryStatsUsage!.ToObject<long>() - cacheMemory;
        var rss = responseBody.GetValue("memory_stats")!.SelectToken("stats")!.SelectToken("rss")!.ToObject<long>();
        var availableMemory = responseBody.GetValue("memory_stats")!.SelectToken("limit")!.ToObject<long>();
        var memoryUsage = (double)usedMemory / (double)availableMemory * 100.0;

        #endregion

        #region CPU

        var cpuStatsTotalUsage = responseBody.GetValue("cpu_stats")!.SelectToken("cpu_usage")!.SelectToken("total_usage")!.ToObject<long>();
        var preCpuStatsTotalUsage = responseBody.GetValue("precpu_stats")!.SelectToken("cpu_usage")!.SelectToken("total_usage")!.ToObject<long>();
        var cpuDelta = cpuStatsTotalUsage - preCpuStatsTotalUsage;
        var cpuStatsSystemCpuUsage = responseBody.GetValue("cpu_stats")!.SelectToken("system_cpu_usage")!.ToObject<long>();
        var preCpuStatsSystemCpuUsage = responseBody.GetValue("precpu_stats")!.SelectToken("system_cpu_usage")!.ToObject<long>();
        var systemCpuDelta = cpuStatsSystemCpuUsage - preCpuStatsSystemCpuUsage;
        var numCpus = responseBody.GetValue("cpu_stats")!.SelectToken("online_cpus")!.ToObject<int>();
        var cpuUsage = (double)cpuDelta / (double)systemCpuDelta * (double)numCpus * 100.0;

        #endregion

        return new ContainerStatus
        {
            AvailableMemoryKb = availableMemory / 1024,
            MemoryKb = usedMemory / 1024,
            RssKb = rss / 1024,
            CacheKb = cacheMemory / 1024,
            MemoryUsage = memoryUsage,
            NumCpus = numCpus,
            CpuUsage = cpuUsage
        };
    }
}
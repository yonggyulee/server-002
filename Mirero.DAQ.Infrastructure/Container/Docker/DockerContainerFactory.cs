using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Mirero.DAQ.Infrastructure.Container.Docker;

public class DockerContainerFactory : IContainerFactory
{
    private readonly HttpClient _httpClient;
    private readonly ICommandResponseErrorHandler _responseErrorHandler;

    public DockerContainerFactory(HttpClient httpClient, ICommandResponseErrorHandler responseErrorHandler)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _responseErrorHandler = responseErrorHandler ?? throw new ArgumentNullException(nameof(responseErrorHandler));
    }

    public async Task<IContainer> CreateAsync(Uri uri, string name, ContainerOptionBuilder option, CancellationToken cancellationToken = default)
    {
        var httpContent = new StringContent(option.Build())
            { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };

        var response = await _httpClient.PostAsync(new Uri(uri, $"containers/create?name={name}"),
            httpContent, cancellationToken);

        if (!response.IsSuccessStatusCode)
            _responseErrorHandler.Handle(await response.Content.ReadAsStringAsync(cancellationToken));

        return new DockerContainer(uri, name, _httpClient, _responseErrorHandler);
    }

    public async Task<IContainer> GetAsync(Uri uri, string name, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(new Uri(uri, $"containers/json?all=true"), cancellationToken);

        if (!response.IsSuccessStatusCode)
            _responseErrorHandler.Handle(await response.Content.ReadAsStringAsync(cancellationToken));

        var responseObject = JObject.Parse(await response.Content.ReadAsStringAsync(cancellationToken));

        var names = responseObject.GetValue("Names")?.Children<JArray>().SingleOrDefault(n => n.ToString().TrimStart('/') == name);
        
        return new DockerContainer(uri, name, _httpClient, _responseErrorHandler);
    }

    public async Task RemoveAsync(Uri uri, string name, CancellationToken cancellationToken = default)
    {
        var httpContent = new StringContent(string.Empty)
            { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } };

        var response = await _httpClient.DeleteAsync(new Uri(uri, $"containers/{name}"), cancellationToken);

        if (!response.IsSuccessStatusCode)
            _responseErrorHandler.Handle(await response.Content.ReadAsStringAsync(cancellationToken));
    }
}
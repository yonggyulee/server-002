using System.Net;
using Newtonsoft.Json.Linq;

namespace Mirero.DAQ.Infrastructure.Container.Docker;

public class DockerCommandResponseErrorHandler : ICommandResponseErrorHandler
{
    public void Handle(string response)
    {
        throw new Exception(JObject.Parse(response).GetValue("message")?.ToString() ?? "Unknown Error");
    }
}
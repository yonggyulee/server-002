using Newtonsoft.Json.Linq;

namespace Mirero.DAQ.Infrastructure.Container.Docker;

public class DockerContainerCreateOption
{
    public string Hostname { get; set; } = "mirero";
    public bool AttachStdin { get; set; } = false;
    public bool AttachStdout { get; set; } = false;
    public bool AttachStderr { get; set; } = false;
    public bool Tty { get; set; } = false;
    public bool OpenStdin { get; set; } = false;
    public List<string> Cmd { get; set; }
    public string Image { get; set; }
    public List<string> Env { get; set; }
    public List<string> Binds { get; set; }
    public string NetworkMode { get; set; } = "host";
    public bool Privileged { get; set; } = false;
    public string RestartPolicyName { get; set; } = "always";
    public bool Init { get; set; } = true;

    public override string ToString()
    {
        var restartPolicy = new JObject { { "Name", RestartPolicyName } };

        var hostConfig = new JObject
        {
            { "Binds", new JArray(Binds) },
            { "NetworkMode", NetworkMode },
            { "Privileged", Privileged },
            { "RestartPolicy", restartPolicy },
            { "Init", Init }
        };

        var jsonObject = new JObject
        {
            { "Hostname", Hostname },
            { "AttachStdin", AttachStdin },
            { "AttachStdout", AttachStdout },
            { "AttachStderr", AttachStderr },
            { "Tty", Tty },
            { "OpenStdin", OpenStdin },
            { "Image", Image },
            { "Env", new JArray(Env) },
            { "Cmd", new JArray(Cmd) },
            { "HostConfig", hostConfig }
        };

        return jsonObject.ToString();
    }
}
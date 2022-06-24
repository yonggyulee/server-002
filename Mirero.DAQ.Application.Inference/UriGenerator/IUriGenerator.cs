namespace Mirero.DAQ.Application.Inference.UriGenerator;

public interface IUriGenerator
{
    string GetModelUri(string volumeUri, string modelName, bool serving = false);
    string GetModelVersionUri(string volumeUri, string modelName, string modelVersion, string filename, bool serving = false);
}
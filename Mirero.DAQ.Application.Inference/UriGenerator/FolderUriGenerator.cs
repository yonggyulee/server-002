using Mirero.DAQ.Infrastructure.Inference;

namespace Mirero.DAQ.Application.Inference.UriGenerator;

public class FolderUriGenerator : IUriGenerator
{
    private readonly string _separator = "_";
    private readonly string _modelVersionPrefix = "";

    public string GetModelUri(string volumeUri, string modelName, bool serving = false)
    {
        var uri = Path.Combine(
            serving ? InferenceWorker.ModelStorePath : "",
            serving ? Path.GetFileName(volumeUri) : volumeUri,
            modelName);

        return serving ? uri.Replace("\\", "/") : uri;
    }

    public string GetModelVersionUri(string volumeUri, string modelName, string modelVersion, string filename, bool serving = false)
    {
        var modelUri = serving
            ? string.Join(_separator, Path.GetFileName(volumeUri), modelName, _modelVersionPrefix + modelVersion,
                filename)
            : Path.Combine(volumeUri, modelName, modelVersion, filename);

        var uri = Path.Combine(serving ? InferenceWorker.ModelStorePath : "", modelUri);

        return serving ? uri.Replace("\\", "/") : uri;
    }
}
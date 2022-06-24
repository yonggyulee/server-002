
namespace Mirero.DAQ.Application.Gds.UriGenerator;

public class FolderUriGenerator : IUriGenerator
{
    public string GetGdsUri(string volumeUri, long gdsId, string gdsFileName)
    {
        return Path.Combine(volumeUri, gdsId.ToString(), gdsFileName);
    }
}
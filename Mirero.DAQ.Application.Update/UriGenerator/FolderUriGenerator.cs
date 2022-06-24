using Mirero.DAQ.Domain.Update.Entity;

namespace Mirero.DAQ.Application.Update.UriGenerator;

public class FolderUriGenerator : IUriGenerator
{
    public Uri GetMppSetupVersionUri(string volumeUri, MppSetupVersion setupVersion)
    {
        return new Uri(Path.Combine(volumeUri, setupVersion.Product, setupVersion.Site, 
            $"MPP_{setupVersion.Year:D4}" +
            $".{setupVersion.Month:D2}" +
            $".{setupVersion.Day:D2}" +
            $".{setupVersion.No}" +
            $"_{setupVersion.Type}"));
    }

    public Uri GetRcSetupVersionUri(string volumeUri, MppSetupVersion setupVersion)
    {
        return new Uri(Path.Combine(volumeUri, setupVersion.Product, setupVersion.Site, 
            $"RC_{setupVersion.Year:D4}" +
            $".{setupVersion.Month:D2}" +
            $".{setupVersion.Day:D2}" +
            $".{setupVersion.No}"));
    }
}
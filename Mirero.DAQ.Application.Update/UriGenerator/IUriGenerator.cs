using Mirero.DAQ.Domain.Update.Entity;

namespace Mirero.DAQ.Application.Update.UriGenerator;

public interface IUriGenerator
{
    Uri GetMppSetupVersionUri(string volumeUri, MppSetupVersion setupVersion);
    Uri GetRcSetupVersionUri(string volumeUri, MppSetupVersion setupVersion);
}
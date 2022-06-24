namespace Mirero.DAQ.Application.Gds.UriGenerator;

public interface IUriGenerator
{
    string GetGdsUri(string volumeUri, long gdsId, string gdsFileName);
}
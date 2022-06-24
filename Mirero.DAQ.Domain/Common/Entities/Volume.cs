namespace Mirero.DAQ.Domain.Common.Entities;

public class Volume
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Uri { get; set; }
    public long Usage { get; set; }
    public long Capacity { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
}
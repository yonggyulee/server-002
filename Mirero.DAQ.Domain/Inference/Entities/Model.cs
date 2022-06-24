namespace Mirero.DAQ.Domain.Inference.Entities;
public class Model
{
    public long Id { get; set; }
    public string VolumeId { get; set; }
    public string TaskName { get; set; }
    public string NetworkName { get; set; }
    public string ModelName { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }

    public Volume Volume { get; set; }
    public DefaultModelVersion DefaultModelVersion { get; set; }
}

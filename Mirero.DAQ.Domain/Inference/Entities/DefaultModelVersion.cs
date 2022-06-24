namespace Mirero.DAQ.Domain.Inference.Entities;

public class DefaultModelVersion
{
    public long Id { get; set; }
    public long ModelId { get; set; }
    public long ModelVersionId { get; set; }

    public Model Model { get; set; }
    public ModelVersion ModelVersion { get; set; }
}
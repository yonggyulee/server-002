namespace Mirero.DAQ.Domain.Inference.Entities;

public class Worker : Common.Entities.Worker
{
    public long ModelVersionId { get; set; }
    public int Port { get; set; }
    public long Usage { get; set; }
    public string ServingType { get; set; }

    public Server Server { get; set; }
    public ModelVersion ModelVersion { get; set; }
}
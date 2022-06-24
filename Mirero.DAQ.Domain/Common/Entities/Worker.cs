namespace Mirero.DAQ.Domain.Common.Entities;

public class Worker
{
    public string Id { get; set; }
    public string ServerId { get; set; }
    public int? CpuCount { get; set; }
    public long?  CpuMemory { get; set; }
    public int GpuCount { get; set; }
    public long GpuMemory { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
}
namespace Mirero.DAQ.Infrastructure.Container;

public class ContainerStatus
{
    public long AvailableMemoryKb { get; set; }
    public long MemoryKb { get; set; }
    public long RssKb { get; set; }
    public long CacheKb { get; set; }
    public double MemoryUsage { get; set; }
    public int NumCpus { get; set; }
    public double CpuUsage { get; set; }
}
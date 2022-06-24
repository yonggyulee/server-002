using System.Net;

namespace Mirero.DAQ.Domain.Common.Entities;

public class Server
{
    public string Id { get; set; }
    public IPAddress Address { get; set; }
    public string OsType { get; set; }
    public string OsVersion { get; set; }
    public int? CpuCount { get; set; }
    public long? CpuMemory { get; set; }
    public string GpuName { get; set; }
    public long GpuCount { get; set; }
    public long GpuMemory { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
}
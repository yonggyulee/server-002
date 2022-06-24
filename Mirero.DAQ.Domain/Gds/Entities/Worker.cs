namespace Mirero.DAQ.Domain.Gds.Entities;

public class Worker : Common.Entities.Worker
{
    public long FloorPlanGdsId { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public FloorPlanGds FloorPlanGds { get; set; }
    public Server Server { get; set; }
}
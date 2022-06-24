namespace Mirero.DAQ.Domain.Gds.Entities;

public class FloorPlanGds
{
    public long Id { get; set; }
    public long FloorPlanId { get; set; }
    public long GdsId { get; set; }
    public string Layers { get; set; }
    public double? OffsetX { get; set; }
    public double? OffsetY { get; set; }
    public FloorPlan FloorPlan { get; set; }
    public Gds Gds { get; set; }
}
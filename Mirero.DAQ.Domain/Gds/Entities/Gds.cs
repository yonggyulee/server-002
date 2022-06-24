namespace Mirero.DAQ.Domain.Gds.Entities;

public class Gds
{
    public long Id { get; set; }
    public string VolumeId { get; set; }
    public string Filename { get; set; }
    public string Extension { get; set; }
    public int? FileSize { get; set; }
    public string Status { get; set; }
    public int? UsingMemorySize { get; set; }
    public string Layers { get; set; }
    public long? StartX { get; set; }
    public long? StartY { get; set; }
    public long? EndX { get; set; }
    public long? EndY { get; set; }
    public double? Dbu { get; set; }
    public int? CellCount { get; set; }
    public int? LayerCount { get; set; }
    public long? ReferenceCount { get; set; }
    public long? ShapeCount{ get; set; }
    public long? EdgeCount{ get; set; }
    public DateTime RegisterDate{ get; set; }
    public DateTime UpdateDate { get; set; }
    public string RegisterUser{ get; set; }
    public string UpdateUser { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
    public Volume Volume { get; set; }
}
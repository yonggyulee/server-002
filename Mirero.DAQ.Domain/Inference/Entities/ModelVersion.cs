namespace Mirero.DAQ.Domain.Inference.Entities;

public class ModelVersion
{
    public long Id { get; set; }
    public long ModelId { get; set; }
    public string Version { get; set; }
    public string Filename { get; set; }
    public DateTime? LoadDate { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string LoadUser { get; set; }
    public string CreateUser { get; set; }
    public string UpdateUser { get; set; }
    public string Status { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }

    public Model Model { get; set; }
}

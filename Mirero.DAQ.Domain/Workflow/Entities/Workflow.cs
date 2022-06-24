namespace Mirero.DAQ.Domain.Workflow.Entities;

public class Workflow
{
    public long Id { get; set; }
    public string VolumeId { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
    public string CreateUser { get; set; }
    public string UpdateUser { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public Volume Volume { get; set; }
    public ICollection<WorkflowVersion> WorkflowVersions { get; set; } = new List<WorkflowVersion>();
}
namespace Mirero.DAQ.Domain.Workflow.Entities;

public class WorkflowVersion
{
    public long Id { get; set; }
    public long WorkflowId { get; set; }
    public string Version { get; set; }
    public string? DataStatus { get; set; }
    public string FileName { get; set; }
    public string Data { get; set; }
    public string CreateUser { get; set; }
    public string UpdateUser { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }

    public Workflow Workflow { get; set; }
}
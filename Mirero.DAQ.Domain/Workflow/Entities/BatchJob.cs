using Mirero.DAQ.Domain.Workflow.Constants;

namespace Mirero.DAQ.Domain.Workflow.Entities;

public class BatchJob
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string WorkflowType { get; set; }
    public int TotalCount { get; set; }
    public string Status { get; set; } = JobStatus.InProgress;
    public DateTime? RegisterDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string RegisterUser { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
}
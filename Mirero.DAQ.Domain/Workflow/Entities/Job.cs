using Mirero.DAQ.Domain.Workflow.Constants;

namespace Mirero.DAQ.Domain.Workflow.Entities;

public class Job
{
    public string Id { get; set; }
    public string BatchJobId { get; set; }
    public string WorkerId { get; set; }
    public string Type { get; set; }
    public long? WorkflowVersionId { get; set; }
    public string Status { get; set; } = JobStatus.Ready;
    public DateTime? RegisterDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public byte[] Parameter { get; set; }

    public BatchJob BatchJob { get; set; }
    public Worker Worker { get; set; }
    public WorkflowVersion WorkflowVersion { get; set; }
}
namespace Mirero.DAQ.Domain.Workflow.Entities;

public class DefaultWorkflowVersion
{
    public long Id { get; set; }
    public long WorkflowId { get; set; }
    public long WorkflowVersionId { get; set; }

    public Workflow Workflow { get; set; }
    public WorkflowVersion WorkflowVersion { get; set; }
}

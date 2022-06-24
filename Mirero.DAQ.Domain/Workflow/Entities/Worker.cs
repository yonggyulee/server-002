namespace Mirero.DAQ.Domain.Workflow.Entities;

public class Worker : Common.Entities.Worker
{
    public string WorkflowType { get; set; }
    public string JobType { get; set; }

    public Server Server { get; set; }
}


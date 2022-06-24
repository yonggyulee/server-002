namespace Mirero.DAQ.Application.Workflow.UriGenerator;

public interface IUriGenerator
{
    string GetWorkflowUri(string volumeUri, long workflowId);
    string GetWorkflowVersionUri(string volumeUri, long workflowId, long workflowversionId);
}
namespace Mirero.DAQ.Application.Workflow.UriGenerator;

public class FolderUriGenerator : IUriGenerator
{
    public string GetWorkflowUri(string volumeUri, long workflowId)
    {
        return Path.Combine(volumeUri, workflowId.ToString());
    }

    public string GetWorkflowVersionUri(string volumeUri, long workflowId, long workflowversionId)
    {
        return Path.Combine(volumeUri, workflowId.ToString(), workflowversionId.ToString());
    }
}
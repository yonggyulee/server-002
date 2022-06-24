namespace Mirero.DAQ.Infrastructure.Redis;

public class NameHandler
{
    private const string JobPrefix = "workflow:job";
    private const string CancelJobPrefix = "workflow:cancel";

    private const string StartJobStreamPrefix = "daq.workflow.start.job";
    private const string UpdateBatchJobStreamPrefix = "daq.workflow.update.batch_job";
    private const string EndJobPubSubPrefix = "daq.workflow.end.job";
    private const string EndBatchJobPubSubPrefix = "daq.workflow.end.batch_job";
    private const string CancelJobPubSubPrefix = "daq.workflow.cancel.job";
    private const string CancelBatchJobPubSubPrefix = "daq.workflow.cancel.batch_job";
    
    public static string GetStartJobStreamName(string jobType)
    {
        return $"{StartJobStreamPrefix}.{jobType}";
    }

    public static string GetUpdateBatchJobStreamName()
    {
        return UpdateBatchJobStreamPrefix;
    }

    public static string GetEndJobPubSubName()
    {
        return EndJobPubSubPrefix;
    }
    
    public static string GetEndBatchJobPubSubName()
    {
        return EndBatchJobPubSubPrefix;
    }
    
    public static string GetCancelJobPubSubName()
    {
        return CancelJobPubSubPrefix;
    }
    
    public static string GetCancelBatchJobPubSubName()
    {
        return CancelBatchJobPubSubPrefix;
    }

    public static string GetCancelJobName(string batchJobId, string? jobId = null)
    {
        return jobId is null ? $"{CancelJobPrefix}:{batchJobId}" : $"{CancelJobPrefix}:{batchJobId}:{jobId}";
    }

    public static string GetBatchJobStringName(string batchJobId)
    {
        return $"{JobPrefix}:{batchJobId}";
    }

    public static string GetJobHashName(string batchJobId, string jobId)
    {
        return $"{JobPrefix}:{batchJobId}:{jobId}";
    }
}
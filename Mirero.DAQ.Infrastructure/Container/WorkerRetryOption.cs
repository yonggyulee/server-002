namespace Mirero.DAQ.Infrastructure.Container;

public class WorkerRetryOption
{
    public int RetryCount { get; } = 1;
    public long RetryDelayMilliseconds { get; } = 1000;
}
namespace Mirero.DAQ.Domain.Workflow.Constants
{
    //fail>timeout>canceled>success
    public class JobStatus
    {
        public const string Ready = "READY";
        public const string InProgress = "INPROGRESS";
        public const string Success = "SUCCESS";
        public const string Fail = "FAIL";
        public const string Cancel = "CANCEL";
        public const string Cancelled = "CANCELLED";
        public const string Timeout = "TIMEOUT";

        public static bool IsEndJob(string jobStatus)
        {
            if(jobStatus == Success 
                || jobStatus == Fail 
                || jobStatus == Cancelled 
                || jobStatus == Timeout)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

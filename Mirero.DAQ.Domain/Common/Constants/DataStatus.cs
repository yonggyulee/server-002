namespace Mirero.DAQ.Domain.Common.Constants;

public class DataStatus
{
    public const string Success = "SUCCESS";
    public const string Fail = "FAIL";

    public static bool IsUploadStatus(string dataStatus)
    {
        return int.TryParse(dataStatus, out _);
    }
}
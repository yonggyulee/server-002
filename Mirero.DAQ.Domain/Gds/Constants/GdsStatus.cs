namespace Mirero.DAQ.Domain.Gds.Constants;

public static class GdsStatus
{
    public const string Success = "Success";
    public const string Fail = "Fail";
    public const string NoFile = "NoFile";
    
    public static string Process(long totalReadBytes, long fileSize)
    {
        var percentComplete = (int)(0.5f + 100f * totalReadBytes / fileSize);
       
        return  percentComplete.ToString();
    }
}
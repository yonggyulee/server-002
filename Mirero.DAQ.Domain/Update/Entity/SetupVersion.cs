namespace Mirero.DAQ.Domain.Update.Entity;

public class SetupVersion
{
    public string Id { get; set; }
    public string Version { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public int No { get; set; }
    public string Type { get; set; }
    public string Product { get; set; }
    public string Site { get; set; }
}
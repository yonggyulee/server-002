namespace Mirero.DAQ.Domain.Gds.Entities;

public class GdsLoadHistory
{
    public long Id { get; set; }
    public string ServerId { get; set; }
    public string FloorPlanId { get; set; }
    public string GdsId { get; set; }
    public string GdsFileName { get; set; }
    public string LoadUserName { get; set; }
    public string UnloadUserName { get; set; }
    public DateTime LoadStartDate { get; set; }
    public DateTime LoadEndDate { get; set; }
    public DateTime UnloadDate { get; set; }
    public string LoadParameters { get; set; }
}
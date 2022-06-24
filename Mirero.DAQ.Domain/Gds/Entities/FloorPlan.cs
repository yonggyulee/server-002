namespace Mirero.DAQ.Domain.Gds.Entities;

public class FloorPlan
{
    public long Id { get; set; }
    public string Title { get; set; }
    public DateTime RegisterDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public string RegisterUser { get; set; }
    public string UpdateUser { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
    public List<FloorPlanGds> FloorPlanGdses { get; set; }
}
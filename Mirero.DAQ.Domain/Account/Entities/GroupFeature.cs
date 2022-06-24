namespace Mirero.DAQ.Domain.Account.Entities;

public class GroupFeature
{
    public long Id { get; set; }
    public string GroupId { get; set; }
    public string FeatureId { get; set; }
    public Group Group { get; set; }
}
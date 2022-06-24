namespace Mirero.DAQ.Domain.Account.Entities;

public class GroupSystem
{
    public string GroupId { get; set; }
    public string SystemId { get; set; }
    public Group Group { get; set; }
    public System System { get; set; }
}
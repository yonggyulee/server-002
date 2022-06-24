namespace Mirero.DAQ.Domain.Account.Entities;

public class UserPrivilege
{
    public long Id { get; set; }
    public string UserId { get; set; }
    public string PrivilegeId { get; set; }
    public User User { get; set; }
    //public Privilege Privilege { get; set; }
}
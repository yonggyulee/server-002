namespace Mirero.DAQ.Domain.Account.Entities;

public class User : ICloneable
{
    public string Id { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    public string Email { get; set; }
    public DateTime RegisterDate { get; set; }
    public bool Enabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; }
    public DateTime LastPasswordChangedDate { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }
    public string GroupId { get; set; }
    public string RoleId { get; set; }
    public Group Group { get; set; }
    //public Role Role { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
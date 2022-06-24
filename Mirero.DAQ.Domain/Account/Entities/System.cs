namespace Mirero.DAQ.Domain.Account.Entities;

public class System : ICloneable
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Properties { get; set; }
    public string Description { get; set; }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
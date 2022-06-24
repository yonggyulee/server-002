namespace Mirero.DAQ.Domain.Account.Entities;

public class RefreshToken
{
    public string Token { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}

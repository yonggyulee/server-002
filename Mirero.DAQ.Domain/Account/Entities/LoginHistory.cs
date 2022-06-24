using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Mirero.DAQ.Domain.Account.Entities;

public class LoginHistory
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public IPAddress AccessIp { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}
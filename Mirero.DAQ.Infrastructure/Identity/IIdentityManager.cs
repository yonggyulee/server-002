namespace Mirero.DAQ.Infrastructure.Identity;

public interface IIdentityManager
{
    void CreateUserAsync(string id, string password, bool enable = false);
}
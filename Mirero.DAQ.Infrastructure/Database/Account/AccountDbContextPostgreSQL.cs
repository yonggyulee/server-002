using Microsoft.EntityFrameworkCore;

namespace Mirero.DAQ.Infrastructure.Database.Account;

public class AccountDbContextPostgreSQL : AccountDbContext
{
    public AccountDbContextPostgreSQL(DbContextOptions<AccountDbContextPostgreSQL> options) : base(options) { }
}
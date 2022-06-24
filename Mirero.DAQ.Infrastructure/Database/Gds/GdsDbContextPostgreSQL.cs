using Microsoft.EntityFrameworkCore;

namespace Mirero.DAQ.Infrastructure.Database.Gds;

public class GdsDbContextPostgreSQL : GdsDbContext
{
    public GdsDbContextPostgreSQL(DbContextOptions<GdsDbContextPostgreSQL> options) : base(options) { }
}
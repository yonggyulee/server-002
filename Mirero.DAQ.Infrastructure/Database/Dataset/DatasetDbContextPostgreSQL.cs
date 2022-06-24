using Microsoft.EntityFrameworkCore;

namespace Mirero.DAQ.Infrastructure.Database.Dataset;

public class DatasetDbContextPostgreSQL : DatasetDbContext
{
    public DatasetDbContextPostgreSQL(DbContextOptions<DatasetDbContextPostgreSQL> options) : base(options)
    {
    }
}
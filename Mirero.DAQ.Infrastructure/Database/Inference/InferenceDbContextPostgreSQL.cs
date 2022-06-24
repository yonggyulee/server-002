using Microsoft.EntityFrameworkCore;

namespace Mirero.DAQ.Infrastructure.Database.Inference;

public class InferenceDbContextPostgreSQL : InferenceDbContext
{
    public InferenceDbContextPostgreSQL(DbContextOptions<InferenceDbContextPostgreSQL> options) : base(options) { }
}
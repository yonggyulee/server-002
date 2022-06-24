using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Gds.Entities;
using Mirero.DAQ.Infrastructure.Database.Gds.EntityConfigurations;


namespace Mirero.DAQ.Infrastructure.Database.Gds;

public class GdsDbContext : DbContext
{
    public DbSet<Volume> Volumes { get; set; } = null!;
    public DbSet<Server> Servers { get; set; } = null!;
    public DbSet<Worker> Workers { get; set; } = null!;
    public DbSet<Domain.Gds.Entities.Gds> Gds { get; set; } = null!;
    public DbSet<FloorPlan> FloorPlan { get; set; } = null!;
    public DbSet<FloorPlanGds> FloorPlansGds { get; set; } = null!;
    public DbSet<GdsLoadHistory> GdsLoadHistory { get; set; } = null!;

    public GdsDbContext(DbContextOptions options) 
        : base(options)
    {
    }

    public GdsDbContext(DbContextOptions<GdsDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("gds");
        builder.ApplyConfiguration(new VolumeEntityTypeConfiguration());
        builder.ApplyConfiguration(new ServerEntityTypeConfiguration());
        builder.ApplyConfiguration(new WorkerEntityTypeConfiguration());
        builder.ApplyConfiguration(new GdsEntityTypeConfigurations());
        builder.ApplyConfiguration(new FloorPlanEntityTypeConfigurations());
        builder.ApplyConfiguration(new FloorPlanGdsEntityTypeConfigurations());
        builder.ApplyConfiguration(new GdsLoadHistoryTypeConfigurations());
    }
}
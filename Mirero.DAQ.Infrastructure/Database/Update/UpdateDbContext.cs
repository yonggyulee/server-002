using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mirero.DAQ.Domain.Update.Entity;
using Mirero.DAQ.Infrastructure.Database.Update.EntityConfigurations;

namespace Mirero.DAQ.Infrastructure.Database.Update;

public class UpdateDbContext : DbContext
{
    public DbSet<Volume> Volumes { get; set; } = null!;
    public DbSet<MppSetupVersion> MppSetupVersions { get; set; } = null!;
    public DbSet<RcSetupVersion> RcSetupVersions { get; set; } = null!;

    public UpdateDbContext(DbContextOptions options) : base (options){}
    public UpdateDbContext(DbContextOptions<UpdateDbContext> options) : base (options){}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new VolumeEntityTypeConfiguration());
        builder.ApplyConfiguration(new MppSetupVersionEntityTypeConfiguration());
        builder.ApplyConfiguration(new RcSetupVersionEntityTypeConfiguration());
    }
}
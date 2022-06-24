using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Update.Entity;

namespace Mirero.DAQ.Infrastructure.Database.Update.EntityConfigurations;

public class MppSetupVersionEntityTypeConfiguration : IEntityTypeConfiguration<MppSetupVersion>
{
    public void Configure(EntityTypeBuilder<MppSetupVersion> builder)
    {
        builder.HasKey(bj => bj.Id);

        builder.Property(bj => bj.Version).IsRequired();
        builder.Property(bj => bj.Year).IsRequired();
        builder.Property(bj => bj.Month).IsRequired();
        builder.Property(bj => bj.Day).IsRequired();
        builder.Property(bj => bj.No).IsRequired();
        builder.Property(bj => bj.Type).IsRequired();
        builder.Property(bj => bj.Product).IsRequired();
        builder.Property(wf => wf.Site).IsRequired();

        builder.ToTable("mpp_setup_version");
    }
}
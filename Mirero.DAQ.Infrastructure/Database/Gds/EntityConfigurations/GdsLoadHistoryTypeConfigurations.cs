using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Gds.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Gds.EntityConfigurations;

public class GdsLoadHistoryTypeConfigurations :IEntityTypeConfiguration<GdsLoadHistory>
{
    public void Configure(EntityTypeBuilder<GdsLoadHistory> builder)
    {
        builder.Property(gh => gh.ServerId).IsRequired();
        builder.Property(gh => gh.FloorPlanId).IsRequired();
        builder.Property(gh => gh.GdsId).IsRequired();
        builder.Property(gh => gh.GdsFileName).HasColumnType("varchar(512)").IsRequired();
        builder.Property(gh => gh.LoadUserName).HasColumnType("varchar(256)").IsRequired();
        builder.Property(gh => gh.UnloadUserName).HasColumnType("varchar(256)").IsRequired();
        builder.Property(gh => gh.LoadStartDate).HasDefaultValueSql("now()").IsRequired();
        builder.Property(gh => gh.LoadEndDate).HasDefaultValueSql("now()").IsRequired();
        builder.Property(gh => gh.UnloadDate).HasDefaultValueSql("now()").IsRequired();
        builder.Property(gh => gh.LoadParameters).HasColumnType("jsonb").IsRequired(false);
        builder.ToTable("gds_load_history");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Gds.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Gds.EntityConfigurations;

public class FloorPlanGdsEntityTypeConfigurations : IEntityTypeConfiguration<FloorPlanGds>
{
    public void Configure(EntityTypeBuilder<FloorPlanGds> builder)
    {
        builder.HasKey(fg => fg.Id);
        builder.Property(fg => fg.Id).ValueGeneratedOnAdd();
        builder.HasOne(fg => fg.FloorPlan).WithMany(fg => fg.FloorPlanGdses).HasForeignKey(fg => fg.FloorPlanId).IsRequired();
        builder.HasOne(fg => fg.Gds).WithMany().HasForeignKey(fg => fg.GdsId).IsRequired();
        builder.Property(fg => fg.Layers).HasColumnType("jsonb").IsRequired();
        builder.Property(fg => fg.OffsetX).IsRequired(false);
        builder.Property(fg => fg.OffsetY).IsRequired(false);
        builder.ToTable("floor_plan_gds");
    }
}
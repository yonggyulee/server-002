using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Gds.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Gds.EntityConfigurations;

public class FloorPlanEntityTypeConfigurations : IEntityTypeConfiguration<FloorPlan>
{
    public void Configure(EntityTypeBuilder<FloorPlan> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedOnAdd();
        builder.Property(f => f.Title).HasColumnType("varchar(512)").IsRequired();
        builder.Property(f => f.RegisterDate).IsRequired();
        builder.Property(f => f.UpdateDate).IsRequired();
        builder.Property(f => f.RegisterUser).HasColumnType("varchar(100)").IsRequired();
        builder.Property(f => f.UpdateUser).HasColumnType("varchar(100)").IsRequired(false);
        builder.Property(f => f.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(f => f.Description).IsRequired(false);
        builder.ToTable("floor_plan");
    }
}
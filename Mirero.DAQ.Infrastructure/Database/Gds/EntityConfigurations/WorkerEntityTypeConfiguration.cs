using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Gds.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Gds.EntityConfigurations;

public class WorkerEntityTypeConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).HasMaxLength(256);
        builder.HasOne(w => w.Server)
            .WithMany()
            .HasForeignKey(w => w.ServerId)
            .IsRequired();
        builder.Property(w => w.CpuCount).IsRequired(false);
        builder.Property(w => w.CpuMemory).IsRequired(false);
        builder.Ignore(w => w.GpuCount);
        builder.Ignore(w => w.GpuMemory);
        builder.Property(w => w.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(w => w.Description).HasColumnType("text").IsRequired(false);
        builder.HasOne(w => w.FloorPlanGds)
            .WithMany()
            .HasForeignKey(w => w.FloorPlanGdsId)
            .IsRequired();
        builder.Property(w => w.CreateDate).HasDefaultValueSql("now()").IsRequired();
        builder.Property(w => w.UpdateDate).HasDefaultValueSql("now()").IsRequired();
        builder.ToTable("worker");
    }
}
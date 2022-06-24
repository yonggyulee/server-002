using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Common.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Common.EntityConfigurations;

public class WorkerEntityTypeConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : Worker
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.HasKey(w => w.Id);
        
        //builder.Property(w => w.Id).HasMaxLength(256);
        builder.Property(w => w.Id).HasColumnType("varchar(256)").IsRequired();
        builder.Property(w => w.CpuCount).HasDefaultValueSql("0").IsRequired();
        builder.Property(w => w.CpuMemory).HasDefaultValueSql("0").IsRequired();
        builder.Property(w => w.GpuCount).HasDefaultValueSql("0").IsRequired();
        builder.Property(w => w.GpuMemory).HasDefaultValueSql("0").IsRequired();
        builder.Property(w => w.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(w => w.Description).HasColumnType("text").IsRequired(false);
    }
}
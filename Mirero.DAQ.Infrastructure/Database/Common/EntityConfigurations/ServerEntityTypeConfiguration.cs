using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Common.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Common.EntityConfigurations;

public class ServerEntityTypeConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : Server
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(256);
        builder.Property(s => s.Address).IsRequired();
        builder.Property(s => s.OsType).HasMaxLength(256).IsRequired();
        builder.Property(s => s.OsVersion).HasMaxLength(256).IsRequired();
        builder.Property(s => s.GpuName).HasMaxLength(256).IsRequired(false);
        builder.Property(s => s.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(s => s.Description).HasColumnType("text").IsRequired(false);
    }
}
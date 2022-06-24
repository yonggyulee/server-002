using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Common.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Common.EntityConfigurations;

public class VolumeEntityTypeConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : Volume
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasMaxLength(256);
        builder.Property(v => v.Title).HasMaxLength(256).IsRequired();
        builder.HasIndex(v => v.Title).IsUnique();
        builder.Property(v => v.Uri).HasMaxLength(256).IsRequired();
        builder.HasIndex(v => v.Uri).IsUnique();
        builder.Property(v => v.Usage).IsRequired().HasDefaultValue(0);
        builder.Property(v => v.Capacity).IsRequired().HasDefaultValue(0);
        builder.Property(v => v.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(v => v.Description).HasColumnType("text").IsRequired(false);
    }
}
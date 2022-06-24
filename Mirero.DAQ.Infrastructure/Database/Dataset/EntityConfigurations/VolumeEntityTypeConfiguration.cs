using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class VolumeEntityTypeConfiguration : IEntityTypeConfiguration<Volume>
{
    public void Configure(EntityTypeBuilder<Volume> builder)
    {
        builder.Property(v => v.Id).HasColumnType("varchar(256)").IsRequired();
        builder.Property(v => v.Type).HasColumnType("varchar(256)").IsRequired();
        builder.Property(v => v.Uri).HasColumnType("varchar(512)").IsRequired();
        builder.Property(v => v.Usage).HasDefaultValueSql("0").IsRequired();
        builder.Property(v => v.Capacity).HasDefaultValueSql("0").IsRequired();
        builder.Property(v => v.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(v => v.Description).HasColumnType("text").IsRequired(false);
        
        builder.HasIndex(v => v.Title).IsUnique();
        builder.HasIndex(v => v.Uri).IsUnique();
        
        builder.ToTable("volume");
    }
}
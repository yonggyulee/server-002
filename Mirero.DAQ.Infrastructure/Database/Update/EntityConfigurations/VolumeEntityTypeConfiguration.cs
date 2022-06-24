using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Update.Entity;

namespace Mirero.DAQ.Infrastructure.Database.Update.EntityConfigurations;

public class VolumeEntityTypeConfiguration : IEntityTypeConfiguration<Volume>
{
    public void Configure(EntityTypeBuilder<Volume> builder)
    {
        builder.Property(v => v.Id).IsRequired();
        builder.Property(v => v.Type).IsRequired();
        builder.Property(v => v.Uri).IsRequired();
        builder.Property(v => v.Usage).IsRequired();
        builder.Property(v => v.Capacity).IsRequired();
        builder.Property(v => v.Properties).IsRequired(false);
        builder.Property(v => v.Description).IsRequired(false);
        
        builder.HasIndex(v => v.Title).IsUnique();
        builder.HasIndex(v => v.Uri).IsUnique();
        
        builder.ToTable("volume");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class ObjectDetectionGtEntityTypeConfiguration : IEntityTypeConfiguration<ObjectDetectionGt>
{
    public void Configure(EntityTypeBuilder<ObjectDetectionGt> builder)
    {
        builder.HasOne(o => o.GtDataset).WithMany().HasForeignKey(o => o.DatasetId).IsRequired();
        builder.HasOne(o => o.Image).WithMany().HasForeignKey(o => o.ImageId).IsRequired();
        
        builder.Property(o => o.Id).ValueGeneratedOnAdd();
        builder.Property(o => o.Filename).HasColumnType("varchar(512)").IsRequired();
        builder.Property(o => o.Extension).HasColumnType("varchar(256)").IsRequired();
        // builder.Property(o => o.ThumbnailBuffer).IsRequired(false);
        // builder.Property(o => o.Buffer).IsRequired(false);
        builder.Property(o => o.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(o => o.Description).HasColumnType("text").IsRequired(false);

        // builder.Ignore(o => o.ThumbnailBuffer);
        builder.Ignore(o => o.Buffer);
        
        builder.ToTable("object_detection_gt");
    }
}
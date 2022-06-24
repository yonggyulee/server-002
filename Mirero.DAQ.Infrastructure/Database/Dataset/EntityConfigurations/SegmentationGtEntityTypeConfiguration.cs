using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class SegmentationGtEntityTypeConfiguration : IEntityTypeConfiguration<SegmentationGt>
{
    public void Configure(EntityTypeBuilder<SegmentationGt> builder)
    {
        builder.HasOne(s => s.GtDataset).WithMany().HasForeignKey(s => s.DatasetId).IsRequired();
        builder.HasOne(s => s.Image).WithMany().HasForeignKey(s => s.ImageId).IsRequired();
        
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        builder.Property(s => s.Filename).HasColumnType("varchar(512)").IsRequired();
        builder.Property(s => s.Extension).HasColumnType("varchar(256)").IsRequired();
        // builder.Property(s => s.ThumbnailBuffer).IsRequired(false);
        // builder.Property(s => s.Buffer).IsRequired(false);
        builder.Property(s => s.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(s => s.Description).HasColumnType("text").IsRequired(false);
        
        // builder.Ignore(s => s.ThumbnailBuffer);
        builder.Ignore(s => s.Buffer);
        
        builder.ToTable("segmentation_gt");
    }
}
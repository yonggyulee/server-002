using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class SegmentationGtDatasetEntityTypeConfiguration : IEntityTypeConfiguration<SegmentationGtDataset>
{
    public void Configure(EntityTypeBuilder<SegmentationGtDataset> builder)
    {
        builder.ToTable("segmentation_gt_dataset");
    }
}
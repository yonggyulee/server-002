using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class ObjectDetectionGtDatasetEntityTypeConfiguration : IEntityTypeConfiguration<ObjectDetectionGtDataset>
{
    public void Configure(EntityTypeBuilder<ObjectDetectionGtDataset> builder)
    {
        builder.ToTable("object_detection_gt_dataset");
    }
}
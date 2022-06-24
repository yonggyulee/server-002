using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class ClassificationGtDatasetEntityTypeConfiguration : IEntityTypeConfiguration<ClassificationGtDataset>
{
    public void Configure(EntityTypeBuilder<ClassificationGtDataset> builder)
    {
        builder.ToTable("classification_gt_dataset");
    }
}
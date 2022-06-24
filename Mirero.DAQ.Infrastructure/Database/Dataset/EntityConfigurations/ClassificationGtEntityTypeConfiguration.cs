using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class ClassificationGtEntityTypeConfiguration : IEntityTypeConfiguration<ClassificationGt>
{
    public void Configure(EntityTypeBuilder<ClassificationGt> builder)
    {
        builder.HasOne(c => c.GtDataset).WithMany().HasForeignKey(c => c.DatasetId).IsRequired();
        builder.HasOne(c => c.Image).WithMany().HasForeignKey(c => c.ImageId).IsRequired();
        builder.HasOne(c => c.ClassCode).WithMany().HasForeignKey(c => c.ClassCodeId).IsRequired();
        
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(d => d.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(d => d.Description).HasColumnType("text").IsRequired(false);

        builder.ToTable("classification_gt");
    }
}
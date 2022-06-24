using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Inference.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Inference.EntityConfigurations;

public class DefaultModelVersionEntityTypeConfiguration : IEntityTypeConfiguration<DefaultModelVersion>
{
    public void Configure(EntityTypeBuilder<DefaultModelVersion> builder)
    {
        builder.HasOne(dmv => dmv.Model).WithOne(m => m.DefaultModelVersion)
            .HasForeignKey<DefaultModelVersion>(dmv => dmv.ModelId).IsRequired();
        builder.HasOne(dmv => dmv.ModelVersion).WithOne().HasForeignKey<DefaultModelVersion>(dmv => dmv.ModelVersionId)
            .IsRequired();

        builder.Property(dmv => dmv.Id).ValueGeneratedOnAdd();

        builder.ToTable("default_model_version");
    }
}
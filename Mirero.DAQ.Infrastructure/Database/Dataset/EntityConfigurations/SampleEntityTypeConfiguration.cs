using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class SampleEntityTypeConfiguration : IEntityTypeConfiguration<Sample>
{
    public void Configure(EntityTypeBuilder<Sample> builder)
    {
        builder.HasKey(s => new {s.Id, s.DatasetId});
        builder.HasOne(s => s.ImageDataset).WithMany().HasForeignKey(s => s.DatasetId).IsRequired();
        builder.Property(v => v.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(v => v.Description).HasColumnType("text").IsRequired(false);

        
        builder.ToTable("sample");
    }
}
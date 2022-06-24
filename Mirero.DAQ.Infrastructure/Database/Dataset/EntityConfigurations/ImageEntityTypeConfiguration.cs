using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class ImageEntityTypeConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasOne(i => i.Sample).WithMany(s => s.Images).HasForeignKey(i => new { i.SampleId, i.DatasetId }).IsRequired();
        
        builder.Property(i => i.Id).ValueGeneratedOnAdd();
        builder.Property(i => i.Filename).HasColumnType("varchar(512)").IsRequired();
        builder.Property(i => i.Extension).HasColumnType("varchar(256)").IsRequired();
        builder.Property(i => i.ImageCode).HasColumnType("varchar(512)").IsRequired(false);
        // builder.Property(i => i.ThumbnailBuffer).IsRequired(false);
        // builder.Property(i => i.Buffer).IsRequired(false);
        builder.Property(c => c.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(c => c.Description).HasColumnType("text").IsRequired(false);
        
        builder.HasIndex(i => new {i.DatasetId, i.Filename}).IsUnique();

        builder.Ignore(i => i.ThumbnailBuffer);
        builder.Ignore(i => i.Buffer);

        builder.ToTable("image");
    }
}
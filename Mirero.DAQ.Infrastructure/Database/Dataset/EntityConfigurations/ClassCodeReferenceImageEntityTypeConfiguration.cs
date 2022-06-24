using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class ClassCodeReferenceImageEntityTypeConfiguration : IEntityTypeConfiguration<ClassCodeReferenceImage>
{
    public void Configure(EntityTypeBuilder<ClassCodeReferenceImage> builder)
    {
        builder.HasOne(c => c.ClassCode).WithMany(c => c.ClassCodeReferenceImages).HasForeignKey(c => c.ClassCodeId).IsRequired();
        //builder.HasOne(c => c.ClassCodeSet).WithMany().HasForeignKey(c => c.ClassCodeSetId).IsRequired();

        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.Filename).HasColumnType("varchar(512)").IsRequired();
        builder.Property(c => c.Extension).HasColumnType("varchar(256)").IsRequired();
        builder.Property(c => c.Description).HasColumnType("text").IsRequired(false);

        builder.HasIndex(c => c.Filename).IsUnique();

        builder.Ignore(c => c.Buffer);

        builder.ToTable("class_code_reference_image");
    }
}
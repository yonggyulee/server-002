using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class ImageDatasetEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Dataset.Entities.ImageDataset>
{
    public void Configure(EntityTypeBuilder<Domain.Dataset.Entities.ImageDataset> builder)
    {
        builder.HasOne(d => d.Volume).WithMany().HasForeignKey(d => d.VolumeId).IsRequired();
        
        builder.Property(d => d.Id).ValueGeneratedOnAdd();
        builder.Property(d => d.Title).HasColumnType("varchar(512)").IsRequired();
        builder.Property(d => d.DirectoryName).HasColumnType("varchar(512)").IsRequired();
        builder.Property(d => d.CreateDate).HasDefaultValueSql("now()");    // Postgresql 함수
        builder.Property(d => d.UpdateDate).HasDefaultValueSql("now()");    // Postgresql 함수
        builder.Property(d => d.CreateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(d => d.UpdateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(d => d.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(d => d.Description).HasColumnType("text").IsRequired(false);
       
        builder.HasIndex(d => d.Title).IsUnique();
        builder.HasIndex(d => new {d.DirectoryName, d.VolumeId}).IsUnique();
        
        builder.Ignore(d => d.ThumbnailBuffer);

        builder.ToTable("image_dataset");
    }
}
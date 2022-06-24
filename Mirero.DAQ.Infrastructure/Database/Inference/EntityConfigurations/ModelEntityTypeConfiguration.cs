using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Inference.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Inference.EntityConfigurations;
public class ModelEntityTypeConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.HasOne(m => m.Volume).WithMany().HasForeignKey(m => m.VolumeId).IsRequired();

        builder.Property(m => m.Id).ValueGeneratedOnAdd();
        builder.Property(m => m.TaskName).HasColumnType("varchar(256)").IsRequired();
        builder.Property(m => m.NetworkName).HasColumnType("varchar(256)").IsRequired();
        builder.Property(m => m.ModelName).HasColumnType("varchar(256)").IsRequired();
        builder.Property(m => m.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(m => m.Description).HasColumnType("text").IsRequired(false);

        builder.HasIndex(m => m.ModelName).IsUnique();

        builder.ToTable("model");
    }
}

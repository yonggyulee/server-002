using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Inference.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Inference.EntityConfigurations;
public class ModelVersionEntityTypeConfiguration : IEntityTypeConfiguration<ModelVersion>
{
    public void Configure(EntityTypeBuilder<ModelVersion> builder)
    {
        builder.HasOne(m => m.Model).WithMany().HasForeignKey(m => m.ModelId).IsRequired();

        builder.Property(m => m.Id).ValueGeneratedOnAdd();
        builder.Property(m => m.Version).HasColumnType("varchar(32)").IsRequired();
        builder.Property(m => m.Filename).HasColumnType("varchar(512)").IsRequired();
        builder.Property(m => m.LoadDate).IsRequired(false);    // Postgresql 함수
        builder.Property(m => m.CreateDate).HasDefaultValueSql("now()").IsRequired();    // Postgresql 함수
        builder.Property(m => m.UpdateDate).HasDefaultValueSql("now()").IsRequired();    // Postgresql 함수
        builder.Property(m => m.LoadUser).HasColumnType("varchar(256)").IsRequired(false);
        builder.Property(m => m.CreateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(m => m.UpdateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(m => m.Status).HasColumnType("varchar(100)").IsRequired(false);
        builder.Property(m => m.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(m => m.Description).HasColumnType("text").IsRequired(false);

        builder.HasIndex(m => new { m.ModelId, m.Version, m.Filename }).IsUnique();

        builder.ToTable("model_version");
    }
}

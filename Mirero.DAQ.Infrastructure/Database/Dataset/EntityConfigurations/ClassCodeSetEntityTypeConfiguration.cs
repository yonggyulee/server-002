using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class ClassCodeSetEntityTypeConfiguration : IEntityTypeConfiguration<ClassCodeSet>
{
    public void Configure(EntityTypeBuilder<ClassCodeSet> builder)
    {
        builder.HasOne(c => c.Volume).WithMany().HasForeignKey(c => c.VolumeId).IsRequired();
        
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.Title).HasColumnType("varchar(512)").IsRequired();
        builder.Property(c => c.Task).HasColumnType("varchar(512)").IsRequired();
        builder.Property(c => c.DirectoryName).HasColumnType("varchar(512)").IsRequired();
        builder.Property(c => c.CreateDate).HasDefaultValueSql("now()");    // Postgresql 함수
        builder.Property(c => c.UpdateDate).HasDefaultValueSql("now()");    // Postgresql 함수
        builder.Property(c => c.CreateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(c => c.UpdateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(c => c.Properties).HasColumnType("jsonb").IsRequired(false); 
        builder.Property(c => c.Description).HasColumnType("text").IsRequired(false); 
        
        builder.HasIndex(c => c.Title).IsUnique();
        builder.HasIndex(c => new {c.DirectoryName, c.VolumeId}).IsUnique();
        
        builder.ToTable("class_code_set");
    }
}
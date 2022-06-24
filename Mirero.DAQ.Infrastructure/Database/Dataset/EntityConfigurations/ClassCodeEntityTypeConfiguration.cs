using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Dataset.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Dataset.EntityConfigurations;

public class ClassCodeEntityTypeConfiguration : IEntityTypeConfiguration<ClassCode>
{
    public void Configure(EntityTypeBuilder<ClassCode> builder)
    {
        builder.HasOne(c => c.ClassCodeSet).WithMany(cs => cs.ClassCodes).HasForeignKey(c => c.ClassCodeSetId).IsRequired();
        
        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.Code).IsRequired();
        builder.Property(c => c.Name).HasColumnType("varchar(512)").IsRequired();
        builder.Property(c => c.CreateDate).HasDefaultValueSql("now()");    // Postgresql 함수
        builder.Property(c => c.UpdateDate).HasDefaultValueSql("now()");    // Postgresql 함수
        builder.Property(c => c.CreateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(c => c.UpdateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(c => c.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(c => c.Description).IsRequired(false);

        builder.ToTable("class_code");
    }
}
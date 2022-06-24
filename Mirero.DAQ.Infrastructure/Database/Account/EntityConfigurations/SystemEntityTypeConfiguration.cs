using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mirero.DAQ.Infrastructure.Database.Account.EntityConfigurations;

public class SystemEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Account.Entities.System> 
{
    public void Configure(EntityTypeBuilder<Domain.Account.Entities.System> builder)
    {
        builder.HasKey(s => s.Id);
        builder.HasIndex(s => s.Title).IsUnique();
        builder.Property(s => s.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(s => s.Description).IsRequired(false);
        builder.Property(s => s.Id).HasMaxLength(256);
        builder.Property(s => s.Title).HasMaxLength(256);
        
        builder.ToTable("system");
    }
}
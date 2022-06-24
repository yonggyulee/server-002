using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Account.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Account.EntityConfigurations;

public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<Group> 
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasIndex(g => g.Title).IsUnique();
        
        builder.Property(g => g.Id).HasMaxLength(256);
        builder.Property(g => g.Title).HasMaxLength(256);
        builder.Property(g => g.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(g => g.Description).IsRequired(false);
        
        builder.ToTable("group");
    }
}
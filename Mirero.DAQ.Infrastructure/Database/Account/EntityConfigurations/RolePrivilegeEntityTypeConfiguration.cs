using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Account.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Account.EntityConfigurations;

public class RolePrivilegeEntityTypeConfiguration : IEntityTypeConfiguration<RolePrivilege> 
{
    public void Configure(EntityTypeBuilder<RolePrivilege> builder)
    {
        builder.HasKey(rp => rp.Id);
        builder.HasKey(rp => new {rp.PrivilegeId, rp.RoleId});
        builder.Property(rp => rp.Id).ValueGeneratedOnAdd();
        builder.Property(rp => rp.RoleId).HasMaxLength(256).IsRequired();
        builder.Property(rp => rp.PrivilegeId).HasMaxLength(256).IsRequired();
  
        
        builder.ToTable("role_privileges");
    }
}
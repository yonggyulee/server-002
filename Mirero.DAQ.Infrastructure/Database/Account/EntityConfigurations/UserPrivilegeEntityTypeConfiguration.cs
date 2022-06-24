using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Account.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Account.EntityConfigurations;

public class UserPrivilegeEntityTypeConfiguration : IEntityTypeConfiguration<UserPrivilege> 
{
    public void Configure(EntityTypeBuilder<UserPrivilege> builder)
    {
        builder.HasKey(up => up.Id);
        builder.Property(up => up.Id).ValueGeneratedOnAdd();
        builder.Property(up => up.PrivilegeId).HasMaxLength(256).IsRequired();
        builder.HasKey(up => new {up.PrivilegeId, up.UserId});
        
        builder.ToTable("user_privileges");
    }
}
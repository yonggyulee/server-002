using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Account.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Account.EntityConfigurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User> 
{
        public void Configure(EntityTypeBuilder<User> builder)
        {
                builder.HasKey(u => u.Id);
                builder.HasIndex(u => u.Name).IsUnique();
                builder.Property(u => u.Password).IsRequired();
                builder.Property(u => u.Department).IsRequired(false);
                builder.Property(u => u.Email).IsRequired(false);
                builder.Property(u => u.Description).IsRequired(false);
                builder.Property(u => u.Properties).IsRequired(false);
                builder.Property(u => u.RegisterDate).IsRequired();
                builder.Property(u => u.Enabled).IsRequired().HasDefaultValue("false");
                builder.Property(u => u.AccessFailedCount).HasDefaultValue(0);
                builder.Property(f => f.Id).HasMaxLength(256);
                builder.Property(f => f.Name).IsRequired(false).HasMaxLength(256);
                builder.Property(f => f.Email).IsRequired(false).HasMaxLength(256);
                
                //builder.HasOne<RefreshToken>().WithOne().HasForeignKey<RefreshToken>(rt => rt.UserId).IsRequired();
                //builder.HasMany<UserPrivilege>().WithOne().HasForeignKey(up => up.UserId).IsRequired();
                //builder.HasOne<ApplicationUserRole>().WithOne().HasForeignKey<ApplicationUserRole>(ur => ur.UserId).IsRequired();
                //builder.HasMany<GroupUser>().WithOne().HasForeignKey(gu => gu.UserId).IsRequired();
        
                builder.ToTable("user");
        }
}
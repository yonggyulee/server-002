using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Account.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Account.EntityConfigurations;

public class LoginHistoryEntityTypeConfiguration : IEntityTypeConfiguration<LoginHistory>
{
    public void Configure(EntityTypeBuilder<LoginHistory> builder)
    {
        builder.HasKey(lh => lh.Id);
        builder.Property(lh => lh.Id).ValueGeneratedOnAdd();
        builder.Property(lh => lh.AccessIp).IsRequired(false);
        builder.ToTable("login_history");
    }
}
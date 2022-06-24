using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Account.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Account.EntityConfigurations;

public class GroupSystemEntityTypeConfiguration : IEntityTypeConfiguration<GroupSystem> 
{
    public void Configure(EntityTypeBuilder<GroupSystem> builder)
    {
        builder.HasKey(gs => new {gs.GroupId, gs.SystemId});
        
        builder.ToTable("group_systems");
    }
}
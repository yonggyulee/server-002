using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Account.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Account.EntityConfigurations;

public class GroupFeatureEntityTypeConfiguration : IEntityTypeConfiguration<GroupFeature> 
{
    public void Configure(EntityTypeBuilder<GroupFeature> builder)
    {
        builder.HasKey(up => up.Id); 
        builder.Property(up => up.Id).ValueGeneratedOnAdd();
        
        builder.Property(gf => gf.FeatureId).HasMaxLength(256);
        builder.HasKey(gf => new {gf.FeatureId, gf.GroupId});
        builder.ToTable("group_features");
    }
}
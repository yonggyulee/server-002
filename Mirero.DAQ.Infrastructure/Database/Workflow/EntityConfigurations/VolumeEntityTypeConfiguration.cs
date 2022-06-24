using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Workflow.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Workflow.EntityConfigurations;

public class VolumeEntityTypeConfiguration : Mirero.DAQ.Infrastructure.Database.Common.EntityConfigurations.
    VolumeEntityTypeConfiguration<Volume>
{
    public override void Configure(EntityTypeBuilder<Volume> builder)
    {
        base.Configure(builder);
        builder.ToTable("volume");
    }
}

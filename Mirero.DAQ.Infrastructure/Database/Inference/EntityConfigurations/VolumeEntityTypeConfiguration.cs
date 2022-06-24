using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Inference.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Inference.EntityConfigurations;

public class VolumeEntityTypeConfiguration : Common.EntityConfigurations.VolumeEntityTypeConfiguration<Volume>
{
    public override void Configure(EntityTypeBuilder<Volume> builder)
    {
        base.Configure(builder);
        builder.ToTable("volume");
    }
}

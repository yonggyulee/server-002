using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Inference.Entities;
using Mirero.DAQ.Infrastructure.Database.Common.EntityConfigurations;

namespace Mirero.DAQ.Infrastructure.Database.Inference.EntityConfigurations;
public class WorkerEntityTypeConfiguration : WorkerEntityTypeConfiguration<Worker>
{
    public override void Configure(EntityTypeBuilder<Worker> builder)
    {
        base.Configure(builder);

        builder.HasOne(w => w.Server).WithMany().HasForeignKey(w => w.ServerId).IsRequired();
        builder.HasOne(w => w.ModelVersion).WithOne().HasForeignKey<Worker>(w => w.ModelVersionId).IsRequired();

        builder.Property(w => w.Usage).IsRequired().HasDefaultValue(0);
        builder.Property(w => w.ServingType).HasColumnType("varchar(256)").IsRequired();

        builder.ToTable("worker");
    }
}

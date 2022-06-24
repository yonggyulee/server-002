using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Workflow.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Workflow.EntityConfigurations;

public class WorkerEntityTypeConfiguration : Common.EntityConfigurations.WorkerEntityTypeConfiguration<Worker>
{
    public override void Configure(EntityTypeBuilder<Worker> builder)
    {
        base.Configure(builder);

        builder.HasOne(w => w.Server)
            .WithMany()
            .HasForeignKey(w => w.ServerId)
            .IsRequired();

        builder.Property(w => w.WorkflowType).HasColumnType("varchar(512)").IsRequired();
        builder.Property(w => w.JobType).HasColumnType("varchar(512)").IsRequired();

        builder.ToTable("worker");
    }
}

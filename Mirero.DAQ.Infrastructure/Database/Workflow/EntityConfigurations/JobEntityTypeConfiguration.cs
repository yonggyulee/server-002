using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Workflow.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Workflow.EntityConfigurations;

public class JobEntityTypeConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(j => j.Id);

        builder.HasOne(j => j.BatchJob)
            .WithMany()
            .HasForeignKey(j => j.BatchJobId)
            .IsRequired();
        builder.HasOne(j => j.Worker)
            .WithMany()
            .HasForeignKey(j => j.WorkerId)
            .IsRequired(false);
        //builder.HasOne(j => j.WorkflowVersion)
        //    .WithMany()
        //    .HasForeignKey(j => j.WorkflowVersionId)
        //    .IsRequired(false);

        builder.Property(j => j.Id).HasColumnType("varchar(256)").IsRequired();
        builder.Property(j => j.Type).HasColumnType("varchar(256)").IsRequired();
        builder.Property(j => j.WorkflowVersionId).IsRequired(false);
        builder.Property(j => j.Status).HasColumnType("varchar(256)").IsRequired();
        builder.Property(j => j.RegisterDate).HasDefaultValueSql("now()").IsRequired(false);
        builder.Property(j => j.StartDate).IsRequired(false);
        builder.Property(j => j.EndDate).IsRequired(false);

        builder.ToTable("job");
    }
}

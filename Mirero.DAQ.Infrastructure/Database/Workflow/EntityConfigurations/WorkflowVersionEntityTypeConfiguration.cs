using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Workflow.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Workflow.EntityConfigurations;

public class WorkflowVersionEntityTypeConfiguration : IEntityTypeConfiguration<WorkflowVersion>
{
    public void Configure(EntityTypeBuilder<WorkflowVersion> builder)
    {
        builder.HasKey(wv => wv.Id);

        builder.HasOne(wv => wv.Workflow)
            .WithMany(w => w.WorkflowVersions)
            .HasForeignKey(wv => wv.WorkflowId)
            .IsRequired();

        builder.Property(wv => wv.Id).ValueGeneratedOnAdd();
        builder.Property(wv => wv.Version).HasColumnType("varchar(512)").IsRequired();
        builder.Property(wv => wv.DataStatus).HasColumnType("varchar(256)").IsRequired(false);
        builder.Property(wv => wv.FileName).HasColumnType("varchar(512)").IsRequired();
        builder.Property(wv => wv.CreateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(wv => wv.UpdateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(wv => wv.CreateDate).HasDefaultValueSql("now()").IsRequired();
        builder.Property(wv => wv.UpdateDate).HasDefaultValueSql("now()").IsRequired();
        builder.Property(wv => wv.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(wv => wv.Description).HasColumnType("text").IsRequired(false);

        builder.HasIndex(wv => new { wv.WorkflowId, wv.Version, wv.FileName });
        builder.Ignore(wv => wv.Data);

        builder.ToTable("workflow_version");
    }
}

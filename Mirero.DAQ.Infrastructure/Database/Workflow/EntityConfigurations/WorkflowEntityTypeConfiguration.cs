using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mirero.DAQ.Infrastructure.Database.Workflow.EntityConfigurations;

public class WorkflowEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Workflow.Entities.Workflow>
{
    public void Configure(EntityTypeBuilder<Domain.Workflow.Entities.Workflow> builder)
    {
        builder.HasOne(wf => wf.Volume)
            .WithMany()
            .HasForeignKey(wf => wf.VolumeId)
            .IsRequired();

        builder.Property(wf => wf.Id).ValueGeneratedOnAdd();
        builder.Property(wf => wf.Type).HasColumnType("varchar(50)").IsRequired();
        builder.Property(wf => wf.Title).HasColumnType("varchar(512)").IsRequired();
        builder.Property(wv => wv.CreateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(wv => wv.UpdateUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(wv => wv.CreateDate).HasDefaultValueSql("now()").IsRequired();
        builder.Property(wv => wv.UpdateDate).HasDefaultValueSql("now()").IsRequired();
        builder.Property(wf => wf.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(wf => wf.Description).HasColumnType("text").IsRequired(false);

        builder.HasIndex(wf => wf.Title).IsUnique();

        builder.ToTable("workflow");
    }
}

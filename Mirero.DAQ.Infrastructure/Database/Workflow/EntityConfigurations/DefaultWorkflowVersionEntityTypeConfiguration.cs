using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Workflow.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Workflow.EntityConfigurations;

public class DefaultWorkflowVersionEntityTypeConfiguration : IEntityTypeConfiguration<DefaultWorkflowVersion>
{
    public void Configure(EntityTypeBuilder<DefaultWorkflowVersion> builder)
    {
        builder.HasKey(dw => dw.Id);

        builder.HasOne(dw => dw.Workflow)
            .WithMany()
            .HasForeignKey(dw => dw.WorkflowId)
            .IsRequired();

        builder.HasOne(dw => dw.WorkflowVersion)
            .WithMany()
            .HasForeignKey(dw => dw.WorkflowVersionId)
            .IsRequired();

        builder.Property(dw => dw.Id).ValueGeneratedOnAdd();

        builder.ToTable("default_workflow_version");
    }
}

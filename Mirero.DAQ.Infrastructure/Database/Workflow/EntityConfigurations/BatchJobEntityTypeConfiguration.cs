using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Workflow.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Workflow.EntityConfigurations;

public class BatchJobEntityTypeConfiguration : IEntityTypeConfiguration<BatchJob>
{
    public void Configure(EntityTypeBuilder<BatchJob> builder)
    {
        builder.HasKey(bj => bj.Id);

        builder.Property(bj => bj.Id).HasColumnType("varchar(256)").IsRequired();
        builder.Property(bj => bj.WorkflowType).HasColumnType("varchar(256)").IsRequired();
        builder.Property(bj => bj.TotalCount).HasDefaultValueSql("0").IsRequired();
        builder.Property(bj => bj.Status).HasColumnType("varchar(256)").IsRequired();
        builder.Property(bj => bj.RegisterDate).HasDefaultValueSql("now()").IsRequired(false);
        builder.Property(bj => bj.StartDate).IsRequired(false);
        builder.Property(bj => bj.EndDate).IsRequired(false);
        builder.Property(bj => bj.RegisterUser).HasColumnType("varchar(256)").IsRequired();
        builder.Property(wf => wf.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(wf => wf.Description).HasColumnType("text").IsRequired(false);

        builder.HasIndex(bj => bj.Title);

        builder.ToTable("batch_job");
    }
}

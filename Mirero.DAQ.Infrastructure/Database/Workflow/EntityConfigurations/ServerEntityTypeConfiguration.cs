using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirero.DAQ.Domain.Workflow.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Workflow.EntityConfigurations;

public class ServerEntityTypeConfiguration : Mirero.DAQ.Infrastructure.Database.Common.EntityConfigurations.
    ServerEntityTypeConfiguration<Server>
{
    public override void Configure(EntityTypeBuilder<Server> builder)
    {
        base.Configure(builder);
        builder.Property(s => s.CpuCount).HasDefaultValueSql("0").IsRequired();
        builder.Property(s => s.CpuMemory).HasDefaultValueSql("0").IsRequired();
        builder.Property(s => s.GpuCount).HasDefaultValueSql("0").IsRequired();
        builder.Property(s => s.GpuMemory).HasDefaultValueSql("0").IsRequired();

        builder.ToTable("server");
    }
}
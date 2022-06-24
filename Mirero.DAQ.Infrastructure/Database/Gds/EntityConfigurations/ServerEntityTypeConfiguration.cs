using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server = Mirero.DAQ.Domain.Gds.Entities.Server;

namespace Mirero.DAQ.Infrastructure.Database.Gds.EntityConfigurations;

public class ServerEntityTypeConfiguration : Mirero.DAQ.Infrastructure.Database.Common.EntityConfigurations.
    ServerEntityTypeConfiguration<Server>
{
    public override void Configure(EntityTypeBuilder<Server> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasMaxLength(256);
        builder.Property(s => s.Address).IsRequired();
        builder.Property(s => s.OsType).HasColumnType("varchar(256)").IsRequired();
        builder.Property(s => s.OsVersion).HasColumnType("varchar(256)").IsRequired();
        builder.Property(s => s.CpuCount).IsRequired(false);
        builder.Property(s => s.CpuMemory).IsRequired(false);
        builder.Ignore(s => s.GpuName);
        builder.Ignore(s => s.GpuCount);
        builder.Ignore(s => s.GpuMemory);
        builder.Property(s => s.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(s => s.Description).HasColumnType("text").IsRequired(false);
        builder.ToTable("server");
    }
}

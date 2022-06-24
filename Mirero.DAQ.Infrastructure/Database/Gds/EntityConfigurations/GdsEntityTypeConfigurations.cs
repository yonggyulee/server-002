using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mirero.DAQ.Infrastructure.Database.Gds.EntityConfigurations;

public class GdsEntityTypeConfigurations : IEntityTypeConfiguration<Domain.Gds.Entities.Gds>
{
    public void Configure(EntityTypeBuilder<Domain.Gds.Entities.Gds> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).ValueGeneratedOnAdd();
        builder.HasOne(g => g.Volume)
            .WithMany()
            .HasForeignKey(g => g.VolumeId)
            .IsRequired(false);
        builder.Property(g => g.Filename).HasColumnType("varchar(512)").IsRequired();
        builder.Property(g => g.Extension).HasColumnType("varchar(20)").IsRequired(false);
        builder.Property(g => g.FileSize).IsRequired(false);
        builder.Property(g => g.Status).HasColumnType("varchar(100)").IsRequired(); 
        builder.Property(g => g.UsingMemorySize).HasDefaultValueSql("0").IsRequired();
        builder.Property(g => g.Layers).HasColumnType("jsonb").IsRequired(false);
        builder.Property(g => g.StartX).IsRequired(false);
        builder.Property(g => g.StartY).IsRequired(false);
        builder.Property(g => g.EndX).IsRequired(false);
        builder.Property(g => g.EndY).IsRequired(false);
        builder.Property(g => g.Dbu).IsRequired(false);
        builder.Property(g => g.CellCount).IsRequired(false);
        builder.Property(g => g.LayerCount).IsRequired(false);
        builder.Property(g => g.ReferenceCount).IsRequired(false);
        builder.Property(g => g.ShapeCount).IsRequired(false);
        builder.Property(g => g.EdgeCount).IsRequired(false);
        builder.Property(g => g.RegisterDate).IsRequired();
        builder.Property(g => g.UpdateDate).IsRequired();
        builder.Property(g => g.RegisterUser).HasColumnType("varchar(100)").IsRequired(); 
        builder.Property(g => g.UpdateUser).HasColumnType("varchar(100)").IsRequired(false);
        builder.Property(g => g.Properties).HasColumnType("jsonb").IsRequired(false);
        builder.Property(g => g.Description).HasColumnType("text").IsRequired(false);
       
        builder.ToTable("gds");
    }
}
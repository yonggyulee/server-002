using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow.EntityConfigurations;
using Mirero.DAQ.Domain.Workflow.Entities;

namespace Mirero.DAQ.Infrastructure.Database.Workflow;

public class WorkflowDbContext : DbContext
{
    public DbSet<Volume> Volumes { get; set; }
    public DbSet<Server> Servers { get; set; }  
    public DbSet<Worker> Workers { get; set; }  
    public DbSet<Domain.Workflow.Entities.Workflow> Workflows { get; set; }
    public DbSet<WorkflowVersion> WorkflowVersions { get; set; }    
    public DbSet<DefaultWorkflowVersion> DefaultWorkflowVersions { get; set; }
    public DbSet<BatchJob> BatchJobs { get; set; }  
    public DbSet<Job> Jobs { get; set; } 

    public WorkflowDbContext(DbContextOptions options) : base(options) { }
    public WorkflowDbContext(DbContextOptions<WorkflowDbContextPostgreSQL> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("workflow");
        builder.ApplyConfiguration(new VolumeEntityTypeConfiguration());
        builder.ApplyConfiguration(new ServerEntityTypeConfiguration());
        builder.ApplyConfiguration(new WorkerEntityTypeConfiguration());
        builder.ApplyConfiguration(new WorkflowEntityTypeConfiguration());
        builder.ApplyConfiguration(new WorkflowVersionEntityTypeConfiguration());
        builder.ApplyConfiguration(new DefaultWorkflowVersionEntityTypeConfiguration());
        builder.ApplyConfiguration(new BatchJobEntityTypeConfiguration());  
        builder.ApplyConfiguration(new JobEntityTypeConfiguration());
    }
}
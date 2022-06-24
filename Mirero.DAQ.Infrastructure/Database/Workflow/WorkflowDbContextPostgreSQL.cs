using Microsoft.EntityFrameworkCore;

namespace Mirero.DAQ.Infrastructure.Database.Workflow;

public class WorkflowDbContextPostgreSQL : WorkflowDbContext
{
    public WorkflowDbContextPostgreSQL(DbContextOptions<WorkflowDbContextPostgreSQL> options) : base(options) { }
}
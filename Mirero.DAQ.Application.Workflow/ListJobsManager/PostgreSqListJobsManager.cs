using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Entities;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.ListJobsManager;

public class PostgreSqListJobsManager : IListJobsManager
{
    private readonly WorkflowDbContext _dbContext;
    
    public PostgreSqListJobsManager(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory)
    {
        _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<(int Count, IEnumerable<Job> Items)> GetJobs(BatchJob batchJob
        , QueryParameter queryParameter
        , CancellationToken cancellationToken)
    {
        return await _dbContext.Jobs
            .Where(x => x.BatchJobId == batchJob.Id)
            .AsNoTracking()
            .AsPagedResultAsync(queryParameter, v => v, cancellationToken);
    }
}
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Entities;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.ListJobsManager;

public class ListJobManagerFactory
{
    private readonly WorkflowDbContext _dbContext;
    private readonly PostgreSqListJobsManager _postgresJobManager;
    private readonly RedisListJobsManager _redisJobManager;
    private BatchJob? _batchJob;
    
    public bool IsCompleted { get; private set; }

    public ListJobManagerFactory(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory, 
        PostgreSqListJobsManager postgresJobManager, 
        RedisListJobsManager redisJobManager)
    {
        _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _postgresJobManager = postgresJobManager ?? throw new ArgumentNullException(nameof(postgresJobManager));
        _redisJobManager = redisJobManager ?? throw new ArgumentNullException(nameof(redisJobManager));
        _batchJob = null;
    }

    public async Task InitializeAsync(string batchJobId, CancellationToken cancellationToken)
    {
        _batchJob = await _dbContext.BatchJobs.FindAsync(
                        new object?[] { batchJobId },
                        cancellationToken: cancellationToken)
                    ?? throw new NotImplementedException();
        
        IsCompleted = _batchJob.Status != JobStatus.InProgress;
    }

    public async Task<(int Count, IEnumerable<Job> Items)> GetJobs(QueryParameter queryParameter, CancellationToken cancellationToken)
    {
        if (_batchJob is null)
        {
            throw new InvalidOperationException("Not Initialized");
        }
        
        IListJobsManager impl = IsCompleted ? _postgresJobManager : _redisJobManager;
        return await impl.GetJobs(_batchJob, queryParameter, cancellationToken);
    }
}
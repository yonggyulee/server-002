using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Redis;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.MonitoringBatchJobs;

public class MonitoringBatchJobsHandler : IRequestHandler<MonitoringBatchJobsCommand, MonitoringBatchJobsResponse>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly Connection _redisConnection;
    public MonitoringBatchJobsHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , Connection redisConnection) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
    }

    public async Task<MonitoringBatchJobsResponse> Handle(MonitoringBatchJobsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        // var batchJobs = _dbContext.BatchJobs.Where(x => request.BatchJobIds.Contains(x.Id)).ToList();
        // if (batchJobs.Count == 0 || batchJobs.All(x => JobStatus.IsEndJob(x.Status)))
        // {
        //     return new Empty();
        // }
        //
    
        //todo: redis pub/sub
        // _endJobSubscriber.OnMessage += message =>
        // {
        //
        // };
        //
        // _endBatchJobSubscriber.OnMessage += message =>
        // {
        //
        // };

        return new MonitoringBatchJobsResponse();
    }
}
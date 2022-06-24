using EFCore.BulkExtensions;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Redis;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.UpdateBatchJob
{
    public class UpdateBatchJobHandler :  IRequestHandler<UpdateBatchJobCommand, Empty>
    {
        private readonly WorkflowDbContext _dbContext;
        private readonly Connection _redisConnection;
        public UpdateBatchJobHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
                , Connection redisConnection) 
        {
            _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _redisConnection = redisConnection?? throw new ArgumentNullException(nameof(redisConnection));
        }

        public async Task<Empty> Handle(UpdateBatchJobCommand request, CancellationToken cancellationToken)
        {
            var batchJob = await _dbContext.BatchJobs.FindAsync(
                               new object?[] { request.BatchJobId},
                               cancellationToken: cancellationToken)
                           ?? throw new NotImplementedException();
            
            //todo:redis get streams and bulkinsert
            //var jobs = (await _jobManager.ListJobs(batchJob.Id, batchJob.TotalCount)).ToList();
            // await _dbContext.BulkInsertAsync(jobs, cancellationToken: cancellationToken);
            
            //todo:batchjob update status
            // var status = GetBatchJobStatus(jobs);
            // batchJob.Status = status;
            await _dbContext.SaveChangesAsync(cancellationToken);

            //todo:redis stream update
            // _endJobPublisher.Publish(message, new PublishOption());

            return new Empty();
        }

        private string GetBatchJobStatus(IReadOnlyCollection<Domain.Workflow.Entities.Job> jobs)
        {
            if (jobs.Any(x => x.Status == JobStatus.Fail))
            {
                return JobStatus.Fail;
            }

            if (jobs.Any(x => x.Status == JobStatus.Timeout))
            {
                return JobStatus.Timeout;
            }

            if (jobs.Any(x => x.Status == JobStatus.Cancelled))
            {
                return JobStatus.Cancelled;
            }

            return JobStatus.Success;
        }
    }
}

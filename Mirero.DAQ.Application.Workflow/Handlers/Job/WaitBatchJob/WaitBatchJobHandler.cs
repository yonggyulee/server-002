using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Redis;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.WaitBatchJob;

public class WaitBatchJobHandler : IRequestHandler<WaitBatchJobCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly Connection _redisConnection;

    public WaitBatchJobHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , Connection redisConnection)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
    }

    public async Task<Empty> Handle(WaitBatchJobCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var responseStream = command.ResponseStream;

        AutoResetEvent exitEvent = new AutoResetEvent(false);

        var response = new WaitBatchJobResponse()
        {
            BatchJobId = request.BatchJobId
        };

        try
        {
            cancellationToken.Register(() =>
            {
                exitEvent.Set();
            });

            //todo: redis pub/sub
            // _endJobSubscriber.OnMessage += async (message) =>
            // {
            //     //response.Status = message.
            //     await responseStream.WriteAsync(response);
            // };
            //
            // _endBatchJobSubscriber.OnMessage += (message) =>
            // {
            //     exitEvent.Set();
            // };

            var batchJob = await _dbContext.BatchJobs.FindAsync(new object?[] { request.BatchJobId }) ?? throw new NotImplementedException();
            if (!JobStatus.IsEndJob(batchJob.Status))
            {
                exitEvent.WaitOne();
            }
        }
        finally
        {
            exitEvent?.Dispose();
        }

        return new Empty();
    }
}
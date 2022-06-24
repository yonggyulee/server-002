using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Infrastructure.Redis;
using StackExchange.Redis;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.CancelBatchJob;

public class CancelBatchJobHandler : IRequestHandler<CancelBatchJobCommand, Empty>
{
    private readonly Connection _redisConnection;
    public CancelBatchJobHandler(Connection redisConnection)
    {
        _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
    }

    public async Task<Empty> Handle(CancelBatchJobCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var database = _redisConnection.CreateDatabase();
        var subscriber = _redisConnection.CreateSubscriber();
        var valueEntries = new[]
        {
            new NameValueEntry("batch_job_id", request.BatchJobId)
        };
        await subscriber.PublishAsync(NameHandler.GetCancelBatchJobPubSubName(),JsonSerializer.Serialize(valueEntries));

        await database.StringSetAsync(NameHandler.GetCancelJobName(request.BatchJobId)
            , RedisValue.EmptyString
            , TimeSpan.MaxValue); //maxValue?
        
        return new Empty();
    }
}
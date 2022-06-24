using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Mirero.DAQ.Infrastructure.Redis;
using StackExchange.Redis;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.CancelJob;
public class CancelJobHandler : IRequestHandler<CancelJobCommand, Empty>
{
    private readonly Connection _redisConnection;
    public CancelJobHandler(Connection redisConnection) 
    {
        _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
    }

    public async Task<Empty> Handle(CancelJobCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var database = _redisConnection.CreateDatabase();
        var subscriber = _redisConnection.CreateSubscriber();
        var valueEntries = new[]
        {
            new NameValueEntry("batch_job_id", request.BatchJobId),
            new NameValueEntry("job_id", request.JobId)
        };
        await subscriber.PublishAsync(NameHandler.GetCancelJobPubSubName(),  JsonSerializer.Serialize(valueEntries));

        await database.StringSetAsync(NameHandler.GetCancelJobName(request.BatchJobId, request.JobId)
            , RedisValue.EmptyString
            , TimeSpan.MaxValue); //maxValue?
        
        return new Empty();
    }
}
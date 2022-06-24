using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Workflow.Entities;
using Mirero.DAQ.Infrastructure.Redis;
using StackExchange.Redis;

namespace Mirero.DAQ.Application.Workflow.ListJobsManager;

public class RedisListJobsManager : IListJobsManager
{
    private readonly Connection _redisConnection;
    public RedisListJobsManager(Connection redisConnection)
    {
        _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
    }
    
    public async Task<(int Count, IEnumerable<Job> Items)> GetJobs(BatchJob batchJob
        , QueryParameter queryParameter
        , CancellationToken cancellationToken)
    {
        var keys = await ScanKeysAsync($"{NameHandler.GetBatchJobStringName(batchJob.Id)}:*"
            , batchJob.TotalCount
            , queryParameter);
            
        (string key, HashEntry[] entries)[] jobs
            = await Task.WhenAll(keys.Select(async x => (x
                , await _redisConnection.CreateDatabase().HashGetAllAsync(x))));

        return ConvertHashEntriesToJob(batchJob.Id, jobs);
    }
    
    private (int, IEnumerable<Job>) ConvertHashEntriesToJob(string batchJobId, (string key, HashEntry[] entries)[] jobs)
    {
        return (jobs.Length,
                jobs.Select(x  =>
                new Job
                {
                    Id = GetJobIdByKey(x.key, batchJobId),
                    BatchJobId = batchJobId,
                    Status = GetRedisValue(x.entries, "status"),
                    Type = GetRedisValue(x.entries, "type"),
                    WorkerId = GetRedisValue(x.entries, "worker_id"),
                    WorkflowVersionId = (long)GetRedisValue(x.entries, "workflow_version_id"),
                    RegisterDate = ConvertDateTime(GetRedisValue(x.entries, "register_date")), 
                    StartDate = ConvertDateTime(GetRedisValue(x.entries, "start_date")),
                    EndDate = ConvertDateTime(GetRedisValue(x.entries, "end_date")),
                    Parameter = (byte[])GetRedisValue(x.entries, "parameter"),
                }));
    }
    
    private async Task<List<string>> ScanKeysAsync(string match, int totalCount, QueryParameter queryParameter)
    {
        var schemas=new List<string>();
        int nextCursor = 0;
        do
        {
            var redisResult = await _redisConnection.CreateDatabase()
                .ExecuteAsync("SCAN", nextCursor.ToString(), "MATCH", match, "COUNT", totalCount);
        
            var innerResult = (RedisResult[])redisResult;
            nextCursor = int.Parse((string)innerResult[0]);
            schemas.AddRange(((string[])innerResult[1]).ToList());
        }
        while (nextCursor != 0);
        
        return schemas
            .OrderBy(x => x)
            .Skip(queryParameter.PageIndex * queryParameter.PageSize)
            .Take(queryParameter.PageSize)
            .ToList();;
    }
    
    private string GetJobIdByKey(string key, string batchJobId)
    {
        return key.Replace($"{NameHandler.GetBatchJobStringName(batchJobId)}:", "");
    }

    private RedisValue GetRedisValue(IEnumerable<HashEntry> entries, string name)
    {
        return entries.FirstOrDefault(x => x.Name == name).Value;
    }

    private DateTime? ConvertDateTime(string redisValue)
    {
        return string.IsNullOrEmpty(redisValue) 
            ? null :  DateTime.SpecifyKind(Convert.ToDateTime(redisValue), DateTimeKind.Utc);
    }
}
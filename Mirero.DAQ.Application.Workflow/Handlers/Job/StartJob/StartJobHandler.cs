using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Application.Workflow.UriGenerator;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Redis;
using StackExchange.Redis;
using JobEntity = Mirero.DAQ.Domain.Workflow.Entities.Job;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.StartJob;

public class StartJobHandler : IRequestHandler<StartJobCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUriGenerator _uriGenerator;
    private readonly Connection _redisConnection;

    public StartJobHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper
            , IUriGenerator uriGenerator
            , Connection redisConnection)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
        _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
    }

    public async Task<Empty> Handle(StartJobCommand command, CancellationToken cancellationToken)
    {
        var requestStream = command.RequestStream;
        var database = _redisConnection.CreateDatabase();

        await foreach (var current in requestStream.ReadAllAsync(cancellationToken))
        {
            var job = _mapper.From(current).AdaptToType<JobEntity>();
            job.RegisterDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            await database
                .HashSetAsync(NameHandler.GetJobHashName(job.BatchJobId, job.Id), CreatHashEntries(job));

            await database
                .StreamAddAsync(NameHandler.GetStartJobStreamName(job.Type),
                    await CreateNameValueEntries(current, cancellationToken));
        }
        
        return new Empty();
    }

    private HashEntry[] CreatHashEntries(JobEntity job)
    {
        return new []
        {
            new HashEntry("status", job.Status),
            new HashEntry("worker_id", job.WorkerId ?? string.Empty),
            new HashEntry("type", job.Type),
            new HashEntry("workflow_version_id", job.WorkflowVersionId),
            new HashEntry("status", job.Status),
            new HashEntry("register_date", job.RegisterDate?.ToString() ?? string.Empty),
            new HashEntry("start_date", job.StartDate?.ToString() ?? string.Empty),
            new HashEntry("end_date", job.EndDate?.ToString() ?? string.Empty),
            new HashEntry("parameter", job.Parameter)
        };
    }

    private async Task<NameValueEntry[]> CreateNameValueEntries(StartJobRequest request, CancellationToken cancellationToken)
    {
        //todo: 캐싱
        var workflowVersion = await _dbContext.WorkflowVersions
                                  .Include(x => x.Workflow)
                                  .ThenInclude(w => w.Volume)
                                  .SingleOrDefaultAsync(x => x.Id == request.WorkflowVersionId, cancellationToken)
                              ?? throw new NotImplementedException();

        var fileUri = Path.Combine(_uriGenerator.GetWorkflowVersionUri(workflowVersion.Workflow.Volume.Uri,
                workflowVersion.WorkflowId, workflowVersion.Id)
            , workflowVersion.FileName);
        var workflowType = workflowVersion.Workflow.Type;
        
        return new[]
        {
            new NameValueEntry("batch_job_id", request.BatchJobId),
            new NameValueEntry("job_id", request.JobId),
            new NameValueEntry("workflow_type", workflowType),
            new NameValueEntry("time_out", request.TimeOut),
            new NameValueEntry("uri", fileUri)
        };
    }
}
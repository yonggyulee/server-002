using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Redis;
using BatchJobEntity = Mirero.DAQ.Domain.Workflow.Entities.BatchJob;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.CreateBatchJob;

public class CreateBatchJobHandler : IRequestHandler<CreateBatchJobCommand, BatchJob>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly Connection _redisConnection;
    private readonly RequesterContext _requesterContext;
    public CreateBatchJobHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper
            , Connection redisConnection
            , RequesterContext requesterContext)
    {
        
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
        _requesterContext = requesterContext ?? throw new ArgumentNullException(nameof(requesterContext));
    }

    public async Task<BatchJob> Handle(CreateBatchJobCommand command, CancellationToken cancellationToken)
    {
        var currentUser = _requesterContext.UserName ?? throw new ArgumentNullException(nameof(_requesterContext));
        var request = command.Request;
        if (IsExists<BatchJobEntity, string>(request.Id))
        { 
            throw new NotImplementedException($"{typeof(BatchJobEntity)} Id already exists.");
        }

        var batchJob = _mapper.From(request).AdaptToType<BatchJobEntity>();
        batchJob.RegisterUser = currentUser;
        batchJob.RegisterDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        
        await _dbContext.BatchJobs.AddAsync(batchJob, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await _redisConnection.CreateDatabase()
            .StringSetAsync(NameHandler.GetBatchJobStringName(batchJob.Id), batchJob.TotalCount.ToString());
        
        return _mapper.From(batchJob).AdaptToType<BatchJob>();
    }
    
    private bool IsExists<TModel, TKey>(TKey key) where TModel : class
    {
        return _dbContext.Find<TModel>(key) != null;
    }
}
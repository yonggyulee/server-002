using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Redis;
using WorkerEntity = Mirero.DAQ.Domain.Workflow.Entities.Worker;

namespace Mirero.DAQ.Application.Workflow.Handlers.Worker.CreateWorker;

public class CreateWorkerHandler : IRequestHandler<CreateWorkerCommand, Domain.Workflow.Protos.V1.Worker>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly Connection _redisConnection;

    public CreateWorkerHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper
            , Connection redisConnection) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _redisConnection = redisConnection?? throw new ArgumentNullException(nameof(redisConnection));
    }

    public async Task<Domain.Workflow.Protos.V1.Worker> Handle(CreateWorkerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        if (IsExists<WorkerEntity, string>(request.Id))
        {
            throw new NotImplementedException($"{typeof(WorkerEntity)} Id already exists.");
        }

        var worker = _mapper.From(request).AdaptToType<WorkerEntity>();
        await _dbContext.Workers.AddAsync(worker, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var database = _redisConnection.CreateDatabase();

        //consumer group이 없다면 추가
        //stream:group = 1:1, same name
        var streamName = NameHandler.GetStartJobStreamName(worker.JobType);
        var notExistGroup = !await database.KeyExistsAsync(streamName)
                       || (await database.StreamGroupInfoAsync(streamName)).All(x=>x.Name!=streamName);
        if (notExistGroup)
        {
            await database.StreamCreateConsumerGroupAsync(NameHandler.GetStartJobStreamName(worker.JobType)
                , NameHandler.GetStartJobStreamName(worker.JobType)
                , createStream: true);       
        }
        
        return _mapper.From(worker).AdaptToType<Domain.Workflow.Protos.V1.Worker>();
    }
    
    private bool IsExists<TModel, TKey>(TKey key) where TModel : class
    {
        return _dbContext.Find<TModel>(key) != null;
    }
}

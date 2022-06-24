using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using ServerEntity = Mirero.DAQ.Domain.Workflow.Entities.Server;

namespace Mirero.DAQ.Application.Workflow.Handlers.Server.CreateServer;

public class CreateServerHandler : IRequestHandler<CreateServerCommand, Domain.Workflow.Protos.V1.Server>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    public CreateServerHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Domain.Workflow.Protos.V1.Server> Handle(CreateServerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        if (IsExists<ServerEntity, string>(request.Id))
        {
            throw new NotImplementedException($"{typeof(ServerEntity)} Id already exists.");
        }

        var server = _mapper.From(request).AdaptToType<ServerEntity>();
        await _dbContext.Servers.AddAsync(server, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);


        return _mapper.From(server).AdaptToType<Domain.Workflow.Protos.V1.Server>();
    }
    
    private bool IsExists<TModel, TKey>(TKey key) where TModel : class
    {
        return _dbContext.Find<TModel>(key) != null;
    }
}

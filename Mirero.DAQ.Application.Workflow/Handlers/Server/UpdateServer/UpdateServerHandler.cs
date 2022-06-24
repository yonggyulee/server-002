using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using ServerEntity = Mirero.DAQ.Domain.Workflow.Entities.Server;


namespace Mirero.DAQ.Application.Workflow.Handlers.Server.UpdateServer;

public class UpdateServerHandler : IRequestHandler<UpdateServerCommand, Domain.Workflow.Protos.V1.Server>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public UpdateServerHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper)
       
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Domain.Workflow.Protos.V1.Server> Handle(UpdateServerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var origin = await _dbContext.Servers.AsNoTracking().SingleOrDefaultAsync(
                 x => x.Id == request.Id,
                cancellationToken: cancellationToken)
            ?? throw new NotImplementedException();
        
        var server = _mapper.From(request).AdaptTo(origin);

        _dbContext.Servers.Update(server);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(server).AdaptToType<Domain.Workflow.Protos.V1.Server>();
    }
}

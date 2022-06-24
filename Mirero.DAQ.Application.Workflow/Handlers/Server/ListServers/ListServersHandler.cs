using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Server.ListServers;

public class ListServersHandler : IRequestHandler<ListServersCommand, ListServersResponse>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public ListServersHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ListServersResponse> Handle(ListServersCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var (count, items) = await _dbContext.Servers
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter, v => _mapper.From(v).AdaptToType<Domain.Workflow.Protos.V1.Server>(),
                cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListServersResponse>();
    }
}

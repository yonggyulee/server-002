using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Worker.ListWorkers;

public class ListWorkersHandler : IRequestHandler<ListWorkersCommand, ListWorkersResponse>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    public ListWorkersHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ListWorkersResponse> Handle(ListWorkersCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var (count, items) = await _dbContext.Workers
             .Include(w => w.Server)
             .AsNoTracking()
             .AsPagedResultAsync(request.QueryParameter, v => _mapper.From(v).AdaptToType<Domain.Workflow.Protos.V1.Worker>(),
                 cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListWorkersResponse>();
    }
}

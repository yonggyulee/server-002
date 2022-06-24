using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.ListWorkflowVersions;

public class ListWorkflowVersionsHandler : IRequestHandler<ListWorkflowVersionsCommand, ListWorkflowVersionsResponse>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public ListWorkflowVersionsHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory, IMapper mapper)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ListWorkflowVersionsResponse> Handle(ListWorkflowVersionsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var (count, items) = await _dbContext.WorkflowVersions
            .Where(x => x.WorkflowId == request.WorkflowId)
            .Include(x => x.Workflow)
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter, v => _mapper.From(v).AdaptToType<WorkflowVersion>(),
               cancellationToken);

        return _mapper.From((request, items, count)).AdaptToType<ListWorkflowVersionsResponse>();
    }
}

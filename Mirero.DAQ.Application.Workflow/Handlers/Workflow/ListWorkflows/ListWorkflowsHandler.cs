using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using Mirero.DAQ.Infrastructure.Locking;
using WorkflowEntity = Mirero.DAQ.Domain.Workflow.Entities.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.ListWorkflows;

public class ListWorkflowsHandler : IRequestHandler<ListWorkflowsCommand, ListWorkflowsResponse>
{
    private readonly IDbContextFactory<WorkflowDbContextPostgreSQL> _dbContextFactory;
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public ListWorkflowsHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
        , IMapper mapper)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ListWorkflowsResponse> Handle(ListWorkflowsCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var (count, items) = await _dbContext.Workflows
            .Include(w => w.Volume)
            .Include(w => w.WorkflowVersions)
            .AsNoTracking()
            .AsPagedResultAsync(request.QueryParameter, cancellationToken);
        
        var workflowsWithVersions = items.Select(GetWorkflowWithVersions);
        var workflowsWithVersionsAndDefaultVersion = await GetDefaultVersion(workflowsWithVersions, cancellationToken);
        
        return _mapper.From((request, withVersions: workflowsWithVersionsAndDefaultVersion, count)).AdaptToType<ListWorkflowsResponse>();
    }

    private async Task<IEnumerable<Domain.Workflow.Protos.V1.Workflow>> GetDefaultVersion(IEnumerable<Domain.Workflow.Protos.V1.Workflow> workflows, CancellationToken cancellationToken)
    {
        return await Task.WhenAll(workflows.Select(async x =>
        {
            await using var ctx = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            var defaultVersionInfo = (await ctx.DefaultWorkflowVersions
                .FirstOrDefaultAsync(d => d.WorkflowId == x.Id, cancellationToken: cancellationToken));
            
            x.DefaultWorkflowVersionId = defaultVersionInfo?.WorkflowVersionId;   
            return x;
        }));
    }
    
    private Domain.Workflow.Protos.V1.Workflow GetWorkflowWithVersions(WorkflowEntity workflowEntity)
    {
        var workflowDto = _mapper.From(workflowEntity).AdaptToType<Domain.Workflow.Protos.V1.Workflow>();
        var workflow = workflowEntity.WorkflowVersions.Select(c => _mapper.From(c).AdaptToType<Domain.Workflow.Protos.V1.WorkflowVersion>());
        workflowDto.WorkflowVersions.AddRange(workflow);
        return workflowDto;
    }
}

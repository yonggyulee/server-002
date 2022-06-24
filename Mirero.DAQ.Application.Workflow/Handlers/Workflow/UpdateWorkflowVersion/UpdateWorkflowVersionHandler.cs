using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using WorkflowVersionEntity = Mirero.DAQ.Domain.Workflow.Entities.WorkflowVersion;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.UpdateWorkflowVersion;

public class UpdateWorkflowVersionHandler : IRequestHandler<UpdateWorkflowVersionCommand, WorkflowVersion>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly RequesterContext _requesterContext;
    public UpdateWorkflowVersionHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper
            , RequesterContext requesterContext) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _requesterContext = requesterContext ?? throw new ArgumentException(nameof(requesterContext));
    }

    public async Task<WorkflowVersion> Handle(UpdateWorkflowVersionCommand command, CancellationToken cancellationToken)
    {
        var currentUser = _requesterContext.UserName ?? throw new ArgumentException(nameof(_requesterContext)); 
        var request = command.Request;
        
        var origin = await _dbContext.WorkflowVersions.AsNoTracking().SingleOrDefaultAsync(
                         x => x.Id == request.Id,
                               cancellationToken: cancellationToken)
                           ?? throw new NotImplementedException();

        origin.UpdateUser = currentUser;
        origin.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        var workflowVersion = _mapper.From(request).AdaptTo(origin);

        _dbContext.WorkflowVersions.Update(workflowVersion);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(workflowVersion).AdaptToType<WorkflowVersion>();
    }
}

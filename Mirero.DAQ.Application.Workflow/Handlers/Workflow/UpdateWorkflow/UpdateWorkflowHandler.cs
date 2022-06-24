using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using WorkflowEntity = Mirero.DAQ.Domain.Workflow.Entities.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.UpdateWorkflow;

public class UpdateWorkflowHandler : IRequestHandler<UpdateWorkflowCommand, Domain.Workflow.Protos.V1.Workflow>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly RequesterContext _requesterContext;

    public UpdateWorkflowHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
        , IMapper mapper
        , RequesterContext requesterContext) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _requesterContext = requesterContext ?? throw new ArgumentException(nameof(requesterContext));
    }

    public async Task<Domain.Workflow.Protos.V1.Workflow> Handle(UpdateWorkflowCommand command, CancellationToken cancellationToken)
    {
        var currentUser = _requesterContext.UserName ?? throw new ArgumentException(nameof(_requesterContext)); 

        var request = command.Request;
        var origin = await _dbContext.Workflows.AsNoTracking().SingleOrDefaultAsync(
                               x => x.Id == request.Id,
                               cancellationToken: cancellationToken)
                           ?? throw new NotImplementedException();

        origin.UpdateUser = currentUser;
        origin.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        var workflow = _mapper.From(request).AdaptTo(origin);

        _dbContext.Workflows.Update(workflow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(workflow).AdaptToType<Domain.Workflow.Protos.V1.Workflow>();
    }
}

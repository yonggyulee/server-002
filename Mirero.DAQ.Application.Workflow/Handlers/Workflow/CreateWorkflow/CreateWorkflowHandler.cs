using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using WorkflowEntity = Mirero.DAQ.Domain.Workflow.Entities.Workflow;


namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.CreateWorkflow;

public class CreateWorkflowHandler : IRequestHandler<CreateWorkflowCommand, Domain.Workflow.Protos.V1.Workflow>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly RequesterContext _requesterContext;
    public CreateWorkflowHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
        , IMapper mapper
        , RequesterContext requesterContext)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _requesterContext = requesterContext ?? throw new ArgumentException(nameof(requesterContext));
    }

    public async Task<Domain.Workflow.Protos.V1.Workflow> Handle(CreateWorkflowCommand command, CancellationToken cancellationToken)
    {
        var currentUserName = _requesterContext.UserName ?? throw new ArgumentNullException(nameof(RequesterContext));

        var request = command.Request;
        var workflow = _mapper.From(request).AdaptToType<WorkflowEntity>();
        workflow.CreateDate = workflow.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        workflow.CreateUser = workflow.UpdateUser = currentUserName;

        await _dbContext.Workflows.AddAsync(workflow, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(workflow).AdaptToType<Domain.Workflow.Protos.V1.Workflow>();
    }
}

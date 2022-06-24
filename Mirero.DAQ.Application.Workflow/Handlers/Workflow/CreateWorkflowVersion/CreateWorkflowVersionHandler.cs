using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Workflow.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using WorkflowVersionEntity = Mirero.DAQ.Domain.Workflow.Entities.WorkflowVersion;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.CreateWorkflowVersion;

public class CreateWorkflowVersionHandler : IRequestHandler<CreateWorkflowVersionCommand, WorkflowVersion>
{
    private readonly WorkflowDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly RequesterContext _requesterContext;
    public CreateWorkflowVersionHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory
            , IMapper mapper
            , RequesterContext requesterContext) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _requesterContext = requesterContext ?? throw new ArgumentException(nameof(requesterContext));
    }

    public async Task<WorkflowVersion> Handle(CreateWorkflowVersionCommand command, CancellationToken cancellationToken)
    {
        var currentUserName = _requesterContext.UserName ?? throw new ArgumentNullException(nameof(RequesterContext));

        var request = command.Request;
        var workflowVersion = _mapper.From(request).AdaptToType<WorkflowVersionEntity>();
        workflowVersion.CreateDate = workflowVersion.UpdateDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        workflowVersion.CreateUser = workflowVersion.UpdateUser = currentUserName;

        await _dbContext.WorkflowVersions.AddAsync(workflowVersion, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.From(workflowVersion).AdaptToType<WorkflowVersion>();
    }
}

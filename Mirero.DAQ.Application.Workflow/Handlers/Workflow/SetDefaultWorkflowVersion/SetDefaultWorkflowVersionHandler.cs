using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;
using DefaultWorkflowEntity = Mirero.DAQ.Domain.Workflow.Entities.DefaultWorkflowVersion;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.SetDefaultWorkflowVersion;

public class SetDefaultWorkflowVersionHandler : IRequestHandler<SetDefaultWorkflowVersionCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;
    
    public SetDefaultWorkflowVersionHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<Empty> Handle(SetDefaultWorkflowVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        if (_dbContext.DefaultWorkflowVersions.Any(x => x.WorkflowId == request.WorkflowId)) //Update
        {
            var updateTarget = await _dbContext.DefaultWorkflowVersions
                .SingleAsync(x => x.WorkflowId == request.WorkflowId, cancellationToken: cancellationToken);
            updateTarget.WorkflowVersionId = request.WorkflowVersionId;

            _dbContext.DefaultWorkflowVersions.Update(updateTarget);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        else //Cretae
        {
            DefaultWorkflowEntity defaultWorkflow
                = new DefaultWorkflowEntity() { WorkflowId = request.WorkflowId, WorkflowVersionId = request.WorkflowVersionId };
            await _dbContext.DefaultWorkflowVersions.AddAsync(defaultWorkflow, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}

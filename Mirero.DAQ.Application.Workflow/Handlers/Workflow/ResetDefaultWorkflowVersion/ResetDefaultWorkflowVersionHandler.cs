using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.ResetDefaultWorkflowVersion;

public class ResetDefaultWorkflowVersionHandler : IRequestHandler<ResetDefaultWorkflowVersionCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;
    public ResetDefaultWorkflowVersionHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<Empty> Handle(ResetDefaultWorkflowVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var deleteTarget = await _dbContext.DefaultWorkflowVersions
           .FirstOrDefaultAsync(x => x.WorkflowVersionId == request.WorkflowId, cancellationToken: cancellationToken)
                       ?? throw new NotImplementedException();

        _dbContext.DefaultWorkflowVersions.Remove(deleteTarget);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}

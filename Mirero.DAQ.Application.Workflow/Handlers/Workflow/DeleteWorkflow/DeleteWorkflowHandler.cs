using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Workflow.DeleteWorkflow;

public class DeleteWorkflowHandler : IRequestHandler<DeleteWorkflowCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;

    public DeleteWorkflowHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<Empty> Handle(DeleteWorkflowCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var deleteTarget = await _dbContext.Workflows.FindAsync(
                            new object?[] { request.WorkflowId },
                            cancellationToken: cancellationToken)
                        ?? throw new NotImplementedException();

        _dbContext.Workflows.Remove(deleteTarget);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}

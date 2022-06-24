using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Domain.Workflow.Constants;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Job.DeleteBatchJob;

public class DeleteBatchJobHandler : IRequestHandler<DeleteBatchJobCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;
    public DeleteBatchJobHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<Empty> Handle(DeleteBatchJobCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var target = await _dbContext.BatchJobs.FindAsync(
                        new object?[] { request.BatchJobId },
                        cancellationToken: cancellationToken)
                    ?? throw new NotImplementedException();

        _dbContext.BatchJobs.Remove(target);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}
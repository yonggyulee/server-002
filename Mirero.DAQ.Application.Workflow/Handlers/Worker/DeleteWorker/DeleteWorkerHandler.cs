using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Worker.DeleteWorker;

public class DeleteWorkerHandler : IRequestHandler<DeleteWorkerCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;
    public DeleteWorkerHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory) 
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<Empty> Handle(DeleteWorkerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var deleteTarget = await _dbContext.Workers.FindAsync(
                            new object?[] { request.WorkerId },
                            cancellationToken: cancellationToken)
                        ?? throw new NotImplementedException();

        //if (_dbContext.Any(x => x.Status == JobStatus.InProgress && x.WorkerId == deleteTarget.Id))
        //{
        //    throw new ArgumentException("진행 중인 작업이 있어 삭제할 수 없습니다.");
        //}

        _dbContext.Workers.Remove(deleteTarget);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}

using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mirero.DAQ.Infrastructure.Database.Workflow;

namespace Mirero.DAQ.Application.Workflow.Handlers.Server.DeleteServer;

public class DeleteServerHandler : IRequestHandler<DeleteServerCommand, Empty>
{
    private readonly WorkflowDbContext _dbContext;
    public DeleteServerHandler(IDbContextFactory<WorkflowDbContextPostgreSQL> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<Empty> Handle(DeleteServerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var selectedServer = await _dbContext.Servers.FindAsync(
                           new object?[] { request.ServerId },
                           cancellationToken: cancellationToken)
                       ?? throw new NotImplementedException();

        _dbContext.Servers.Remove(selectedServer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}

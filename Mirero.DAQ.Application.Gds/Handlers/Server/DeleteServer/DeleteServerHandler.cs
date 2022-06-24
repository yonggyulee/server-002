using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Infrastructure.Database.Gds;
using ServerDto = Mirero.DAQ.Domain.Gds.Protos.V1.Server;
using ServerEntity = Mirero.DAQ.Domain.Gds.Entities.Server;

namespace Mirero.DAQ.Application.Gds.Handlers.Server.DeleteServer;

public class DeleteServerHandler : IRequestHandler<DeleteServerCommand, Empty>
{
    private readonly ILogger _logger;
    private readonly GdsDbContext _dbContext;

    public DeleteServerHandler(ILogger<DeleteServerHandler> logger,
        IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<Empty> Handle(DeleteServerCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var serverEntity = new ServerEntity
        {
            Id = request.ServerId
        };

        _dbContext.Remove(serverEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}
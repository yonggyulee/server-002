using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;

namespace Mirero.DAQ.Application.Account.Handlers.Group.DeleteGroup;

public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, Empty>
{
    private readonly ILogger _logger;
    private readonly AccountDbContextPostgreSQL _dbContext;


    public DeleteGroupHandler(ILogger<DeleteGroupHandler> logger,
        IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }


    public async Task<Empty> Handle(DeleteGroupCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var deleteGroup =
            await _dbContext.Groups.SingleOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken: cancellationToken)
            ?? throw new Exception($"{request.GroupId} doesn't exist.");

        _dbContext.Groups.Remove(deleteGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new Empty();
    }
}
using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;

namespace Mirero.DAQ.Application.Account.Handlers.Group.DeleteSystem
{
    public class DeleteSystemHandler : IRequestHandler<DeleteSystemCommand, Empty>
    {
        private readonly ILogger _logger;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public DeleteSystemHandler(ILogger<DeleteSystemHandler> logger,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<Empty> Handle(DeleteSystemCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var deleteSystem = await _dbContext.Systems.SingleOrDefaultAsync(s => s.Id == request.SystemId, cancellationToken)
                               ?? throw new Exception($"{request.SystemId} doesn't exist.");

            _dbContext.Systems.Remove(deleteSystem);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new Empty();
        }
    }
}

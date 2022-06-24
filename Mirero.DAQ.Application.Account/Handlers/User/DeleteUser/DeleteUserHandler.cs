using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;

namespace Mirero.DAQ.Application.Account.Handlers.User.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Empty>
    {
        private readonly ILogger _logger;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public DeleteUserHandler(ILogger<DeleteUserHandler> logger,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<Empty> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var deleteUser = _dbContext.Users.SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken).Result
                             ?? throw new Exception($"{request.UserId} doesn't exist.");

            _dbContext.Users.Remove(deleteUser);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new Empty();
        }
    }
}

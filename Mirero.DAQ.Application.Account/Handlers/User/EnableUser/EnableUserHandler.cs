﻿using System;
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

namespace Mirero.DAQ.Application.Account.Handlers.User.EnableUser
{
    public class EnableUserHandler : IRequestHandler<EnableUserCommand, Empty>
    {
        private readonly ILogger _logger;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public EnableUserHandler(ILogger<EnableUserHandler> logger,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<Empty> Handle(EnableUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var selectedUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (selectedUser != null)
            {
                selectedUser.Enabled = true;
                _dbContext.Users.Update(selectedUser);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return new Empty();
        }
    }
}

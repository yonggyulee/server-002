using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Account.Constants;
using Mirero.DAQ.Domain.Account.Entities;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using PrivilegeDto = Mirero.DAQ.Domain.Account.Protos.V1.Privilege;

namespace Mirero.DAQ.Application.Account.Handlers.User.UpdateUserPrivilege
{
    public class UpdateUserPrivilegeHandler : IRequestHandler<UpdateUserPrivilegeCommand, UpdateUserPrivilegeResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public UpdateUserPrivilegeHandler(ILogger<UpdateUserPrivilegeHandler> logger, IMapper mapper, 
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<UpdateUserPrivilegeResponse> Handle(UpdateUserPrivilegeCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var listPrivileges = typeof(PrivilegeId).GetFields().Select(p => p.Name);
            var enabledPrivileges = request.Privileges
                .Where(p => p.Enabled.Equals(true))
                .Select(p => p.Id).ToList();

            var deleteUser = _dbContext.UserPrivileges.Where(up => up.UserId == request.UserId).ToList()
                             ?? throw new Exception($"{request.UserId} doesn't exist.");

            _dbContext.UserPrivileges.RemoveRange(deleteUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var userPrivileges = enabledPrivileges
                .Select(p => { return _mapper.From((request.UserId, p)).AdaptToType<UserPrivilege>(); });

            await _dbContext.UserPrivileges.AddRangeAsync(userPrivileges, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var privilegeDtos = listPrivileges
                .Select(
                    p => enabledPrivileges.Contains(p)
                        ? _mapper.From((p, p, true)).AdaptToType<PrivilegeDto>()
                        : _mapper.From((p, p, false)).AdaptToType<PrivilegeDto>()
                );

            return _mapper.From(privilegeDtos).AdaptToType<UpdateUserPrivilegeResponse>();
        }
    }
}

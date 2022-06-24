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
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Domain.Common.Extensions;
using Mirero.DAQ.Infrastructure.Database.Account;
using UserDto = Mirero.DAQ.Domain.Account.Protos.V1.User;
using PrivilegeDto = Mirero.DAQ.Domain.Account.Protos.V1.Privilege;

namespace Mirero.DAQ.Application.Account.Handlers.User.ListUsers
{
    public class ListUsersHandler : IRequestHandler<ListUsersCommand, ListUsersResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public ListUsersHandler(ILogger<ListUsersHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }


        public async Task<ListUsersResponse> Handle(ListUsersCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var queryResult = _dbContext.Users
                .OrderBy(u => u.Id)
                .AsNoTracking()
                .AsPagedResult(request.QueryParameter);

            var privileges = typeof(PrivilegeId).GetFields().Select(p => p.Name).ToList();
            var userDtos = new List<UserDto>();
            foreach (var user in queryResult.Items)
            {
                var oneUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == user.Id, cancellationToken) ??
                              throw new NotImplementedException();

                var userPrivileges = _dbContext.UserPrivileges.Where(up => up.UserId == user.Id)
                    .Select(up => up.PrivilegeId).ToList();

                var privilegeDtos = privileges
                    .Select(
                        p => userPrivileges.Contains(p)
                            ? _mapper.From((p, p, true)).AdaptToType<PrivilegeDto>()
                            : _mapper.From((p, p, false)).AdaptToType<PrivilegeDto>()
                    );
                userDtos.Add(_mapper.From((oneUser, privilegeDtos)).AdaptToType<UserDto>());
            }

            //var test = _mapper.From((request, userDtos, queryResult.Count)).AdaptToType<ListUsersResponse>();

            return _mapper.From((request, userDtos, queryResult.Count)).AdaptToType<ListUsersResponse>();

        }
    }
}

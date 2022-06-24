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
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using Mirero.DAQ.Infrastructure.Identity;
using PrivilegeDto = Mirero.DAQ.Domain.Account.Protos.V1.Privilege;
using UserEntity = Mirero.DAQ.Domain.Account.Entities.User;

namespace Mirero.DAQ.Application.Account.Handlers.User.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Domain.Account.Protos.V1.User>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public CreateUserHandler(ILogger<CreateUserHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<Domain.Account.Protos.V1.User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var user = _mapper.From(request).AdaptToType<UserEntity>();

            if (_dbContext.Users.SingleOrDefaultAsync(u => u.Id == user.Id, cancellationToken).Result == null)
            {
                var newName = string.Empty;
                if (_dbContext.Users.SingleOrDefaultAsync(u => u.Name == user.Name, cancellationToken).Result != null)
                {
                    for (var alpha = 65; alpha < 91; alpha++)
                    {
                        var newNameTest = user.Name + Convert.ToChar(alpha);
                        if (_dbContext.Users.SingleOrDefaultAsync(u => u.Name == newNameTest, cancellationToken).Result == null)
                        {
                            newName = newNameTest;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(newName))
                    {
                        throw new Exception("[A-Z] Out of range");
                    }
                }

                if (newName != string.Empty)
                {
                    user.Name = newName;
                }

                user.Password = Encrypt.HashToSHA256(user.Password);
                user.LockoutEnd = null;
                user.RegisterDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                user.Department = user.Department.ToUpper();
                user.LastPasswordChangedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                if (user.RoleId == "SuperAdministrator")
                {
                    user.Enabled = true;
                }
                else
                {
                    user.Enabled = false;
                }

                await _dbContext.Users.AddAsync(user, cancellationToken);

                var privilegeIds = _dbContext.RolePrivileges
                    .Where(rp => rp.RoleId == user.RoleId)
                    .Select(rp => rp.PrivilegeId)
                    .ToList();

                foreach (var privilegeId in privilegeIds)
                {
                    await _dbContext.UserPrivileges.AddAsync(new UserPrivilege
                    { UserId = user.Id, PrivilegeId = privilegeId }, cancellationToken);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Exception($"UserId [{user.Id}] is already registered.");
                _logger.LogWarning($"UserId {user.Id} is already registered.");
            }

            var result = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == user.Id, cancellationToken) ??
                         throw new NotImplementedException();

            var privileges = typeof(PrivilegeId).GetFields().Select(p => p.Name).ToList();
            var userPrivileges = _dbContext.UserPrivileges.Where(up => up.UserId == request.Id).Select(up => up.PrivilegeId).ToList();

            var privilegeDtos = privileges
                .Select(
                    p => userPrivileges.Contains(p)
                        ? _mapper.From((p, p, true)).AdaptToType<PrivilegeDto>()
                        : _mapper.From((p, p, false)).AdaptToType<PrivilegeDto>()
                );

            return _mapper.From((result, privilegeDtos)).AdaptToType<Domain.Account.Protos.V1.User>();
        }
    }
}

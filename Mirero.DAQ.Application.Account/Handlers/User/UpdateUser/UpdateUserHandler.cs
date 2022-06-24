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
using UserEntity = Mirero.DAQ.Domain.Account.Entities.User;
using PrivilegeDto = Mirero.DAQ.Domain.Account.Protos.V1.Privilege;
using UserDto = Mirero.DAQ.Domain.Account.Protos.V1.User;

namespace Mirero.DAQ.Application.Account.Handlers.User.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public UpdateUserHandler(ILogger<UpdateUserHandler> logger, IMapper mapper,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        public async Task<UserDto> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var selectedUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
                    ?? throw new Exception($"{request.Id} doesn't exist.");

            var user = _mapper.From(request).AdaptToType<UserEntity>();

            selectedUser.Name = user.Name == null ? selectedUser.Name : user.Name;
            selectedUser.Department = user.Department == null ? selectedUser.Department : user.Department;
            selectedUser.Email = user.Email == null ? selectedUser.Email : user.Email;
            selectedUser.Enabled = user.Enabled == null ? selectedUser.Enabled : user.Enabled;
            selectedUser.GroupId = user.GroupId == string.Empty ? selectedUser.GroupId : user.GroupId;
            selectedUser.Properties = user.Properties == null ? selectedUser.Properties : user.Properties;
            selectedUser.Description = user.Department == null ? selectedUser.Description : user.Description;
            selectedUser.RegisterDate = selectedUser.RegisterDate;

            if (selectedUser.RoleId != user.RoleId)
            {
                selectedUser.RoleId = user.RoleId;
                var resetUser = _dbContext.UserPrivileges.Where(up => up.UserId == request.Id).ToList();
                _dbContext.UserPrivileges.RemoveRange(resetUser);

                var userRolePrivileges = _dbContext.RolePrivileges
                    .Where(rp => rp.RoleId == selectedUser.RoleId)
                    .Select(rp => rp.PrivilegeId).ToList();

                var updateUserPrivileges = userRolePrivileges
                    .Select(p => { return _mapper.From((request.Id, p)).AdaptToType<UserPrivilege>(); });

                await _dbContext.UserPrivileges.AddRangeAsync(updateUserPrivileges, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            _dbContext.Users.Update(selectedUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

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

            return _mapper.From((result, privilegeDtos)).AdaptToType<UserDto>();
        }
    }
}

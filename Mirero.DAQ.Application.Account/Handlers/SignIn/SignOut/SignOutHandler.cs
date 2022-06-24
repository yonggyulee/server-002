using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using Mirero.DAQ.Infrastructure.Identity;
using UserEntity = Mirero.DAQ.Domain.Account.Entities.User;

namespace Mirero.DAQ.Application.Account.Handlers.SignIn.SignOut
{
    public class SignOutHandler : IRequestHandler<SignOutCommand, Empty>
    {
        private readonly ILogger _logger;
        private readonly AccountDbContextPostgreSQL _dbContext;

        public SignOutHandler(ILogger<SignOutHandler> logger,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        }

        private ClaimsIdentity _ClaimsIdentity(
            UserEntity user,
            IEnumerable<string>? userPrivileges,
            IEnumerable<string>? groupSystems,
            IEnumerable<string>? groupFeatures)
        {
            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claimsIdentity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, user.Name));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.RoleId));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.GroupSid, user.GroupId));

            if (userPrivileges != null) claimsIdentity.AddClaims(userPrivileges.Select(up => new Claim(up, "Y")));
            if (groupSystems != null) claimsIdentity.AddClaims(groupSystems.Select(gs => new Claim(gs, "Y")));
            if (groupFeatures != null) claimsIdentity.AddClaims(groupFeatures.Select(gf => new Claim(gf, "Y")));

            return claimsIdentity;
        }


        public async Task<Empty> Handle(SignOutCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var signOutUser = _dbContext.RefreshTokens
                                  .SingleOrDefaultAsync(rt => rt.UserId == request.UserId, cancellationToken).Result
                              ?? throw new Exception($"{request.UserId} doesn't exist.");

            _dbContext.RefreshTokens.Remove(signOutUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new Empty();
        }
    }
}

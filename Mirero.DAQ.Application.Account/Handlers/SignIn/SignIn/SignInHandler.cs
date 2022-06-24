using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Mirero.DAQ.Domain.Account.Entities;
using Mirero.DAQ.Domain.Account.Protos.V1;
using Mirero.DAQ.Domain.Common.Data;
using Mirero.DAQ.Infrastructure.Database.Account;
using Mirero.DAQ.Infrastructure.Identity;
using UserEntity = Mirero.DAQ.Domain.Account.Entities.User;

namespace Mirero.DAQ.Application.Account.Handlers.SignIn.SignIn
{
    public class SignInHandler : IRequestHandler<SignInCommand, SignInResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly AccountDbContextPostgreSQL _dbContext;
        private readonly ITokenManager _tokenManager;

        public SignInHandler(ILogger<SignInHandler> logger, IMapper mapper, IConfiguration configuration,
            IDbContextFactory<AccountDbContextPostgreSQL> dbContextFactory, ITokenManager tokenManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _dbContext = dbContextFactory?.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
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

        public async Task<SignInResponse> Handle(SignInCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var user = _dbContext.Users.SingleOrDefaultAsync(
    u => u.Id == request.UserId, cancellationToken)?.Result ?? throw new Exception("Incorrect UserID");

            if (user.Password != Encrypt.HashToSHA256(request.Password)) throw new Exception("Incorrect Password");

            if (user.Enabled == false)
                throw new Exception($"[{user.Id}] is not enabled ");

            var claimsIdentity = _ClaimsIdentity(user,
                _dbContext.UserPrivileges.Where(up => up.UserId == user.Id).Select(up => up.PrivilegeId),
                _dbContext.GroupSystems.Where(gs => gs.GroupId == user.GroupId).Select(gs => gs.SystemId),
                _dbContext.GroupFeatures.Where(gf => gf.GroupId == user.GroupId).Select(gf => gf.FeatureId));

            var accessToken = _tokenManager.GenerateAccessToken(claimsIdentity);
            var refreshToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.UserId == user.Id,
                cancellationToken: cancellationToken);

            if (refreshToken == null)
            {
                refreshToken = _tokenManager.GenerateRefreshToken(user.Id);
                var entityEntry = await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                refreshToken = entityEntry.Entity;
            }

            var userExpiryDate = DateTime.Now.AddMinutes(_configuration.GetValue<double>("JwtOption:ExpiryMinutes"));
            if (refreshToken.ExpiryDate < userExpiryDate) // TODO : Option
            {
                var updateUser = _dbContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.UserId == user.Id, cancellationToken).Result
                                 ?? throw new Exception($"{user.Id} doesn't exist.");
                updateUser.Token = refreshToken.Token;
                updateUser.UserId = user.Id;
                updateUser.CreationDate = refreshToken.CreationDate;
                updateUser.ExpiryDate = DateTime.SpecifyKind(DateTime.Now.AddDays(1), DateTimeKind.Utc); // TODO : Option
                var entityEntry = _dbContext.RefreshTokens.Update(updateUser);
                await _dbContext.SaveChangesAsync(cancellationToken);

                refreshToken = entityEntry.Entity;
            }

            await _dbContext.LoginHistories.AddAsync(new LoginHistory
            {
                Date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                UserId = request.UserId
            }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
            var result = _mapper.From((accessToken, refreshToken)).AdaptToType<SignInResponse>();
            return result;
        }
    }
}

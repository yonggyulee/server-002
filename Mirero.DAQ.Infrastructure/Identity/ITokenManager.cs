using System.Security.Claims;
using Mirero.DAQ.Domain.Account.Entities;

namespace Mirero.DAQ.Infrastructure.Identity;

public interface ITokenManager
{
    string GenerateAccessToken(ClaimsIdentity claimsIdentity);
    RefreshToken GenerateRefreshToken(string userId);
    public string? GetAccessTokenClaimValue(string token, string type);
    bool VerifyAccessToken(string token);
}
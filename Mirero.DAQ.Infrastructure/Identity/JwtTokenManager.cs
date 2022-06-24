using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Mirero.DAQ.Domain.Account.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;


namespace Mirero.DAQ.Infrastructure.Identity;

public class JwtTokenManager : ITokenManager
{
    private readonly ILogger<JwtTokenManager> _logger;
    private readonly IConfiguration _configuration;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly SymmetricSecurityKey _secretKey;

    public JwtTokenManager(ILogger<JwtTokenManager> logger, IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _tokenHandler = new();
        _secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
            _configuration.GetValue<string>("JwtOption:SecretKey")));
    }

    public string GenerateAccessToken(ClaimsIdentity claimsIdentity)
    {
        claimsIdentity.AddClaims(new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(CultureInfo.CurrentCulture)),
        });

        var token = _tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _configuration.GetValue<string>("JwtOption:Issuer"),
            Audience = _configuration.GetValue<string>("JwtOption:Audience"),
            Subject = claimsIdentity,
            Expires = DateTime.Now.AddMinutes(_configuration.GetValue<double>("JwtOption:DurationMinutes")),
            SigningCredentials = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256)
        });

        return _tokenHandler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(string userId)
    {
        return new RefreshToken
        {
            Token = Guid.NewGuid().ToString("N"),
            UserId = userId,
            CreationDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc), // TODO : DB Time
            ExpiryDate = DateTime.SpecifyKind(DateTime.Now.AddDays(1), DateTimeKind.Utc), // TODO : From Config
        };
    }

    public string? GetAccessTokenClaimValue(string token, string type)

    {
        if (string.IsNullOrWhiteSpace(token)) return null;
        var jwtToken = _tokenHandler.ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken;
        var claim = jwtToken?.Claims.FirstOrDefault(c => c.Type == type);

        return claim?.Value;
    }

    public bool VerifyAccessToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;

        try
        {
            _tokenHandler.ValidateToken(
                token.Replace("\"", string.Empty),
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _secretKey,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);
        }
        catch (SecurityTokenException)
        {
            return false;
        }

        return true;
    }
}
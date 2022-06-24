using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Mirero.DAQ.Infrastructure.Locking;

public interface IPostgresLockProviderFactory
{
    ILockProvider CreateLockProvider(DbContext dbContext, ILogger? logger = null);
}

public class AdvisoryLockProviderFactory : IPostgresLockProviderFactory
{
    private readonly ILogger<AdvisoryLockProviderFactory> _logger;
    private readonly TimeSpan _lockTimeout;

    public AdvisoryLockProviderFactory(IConfiguration configuration, ILogger<AdvisoryLockProviderFactory> logger)
    {
        _logger = logger;
        var timeoutSeconds = double.Parse(configuration["Lock:Timeout"]);
        _lockTimeout = TimeSpan.FromSeconds(timeoutSeconds > 0 ? timeoutSeconds : 0);
    }

    public ILockProvider CreateLockProvider(DbContext dbContext, ILogger? logger = null)
    {
        return new AdvisoryLockProvider(dbContext, _lockTimeout, logger ?? _logger);
    }
}
using StackExchange.Redis;

namespace Mirero.DAQ.Infrastructure.Redis;

public class Connection : IDisposable
{
    private readonly ConnectionMultiplexer _redis;
    private string _connectionString;
    public Connection(ConnectionConfig connectionConfig)
    {
        var uri = connectionConfig.Uris.FirstOrDefault() ?? throw new ArgumentNullException(nameof(Connection));
        var options = GetConfigurationOptions(uri);

        _redis = ConnectionMultiplexer.Connect(options);
    }

    private ConfigurationOptions GetConfigurationOptions(Uri uri)
    {
        _connectionString = $"{uri.Host}:{uri.Port}";

        var options = ConfigurationOptions.Parse(_connectionString);
        options.Password = uri.UserInfo.Split(':')[1];

        return options;
    }

    public IDatabase CreateDatabase()
    {
        return _redis.GetDatabase();
    }

    public ISubscriber CreateSubscriber()
    {
        return _redis.GetSubscriber();
    }

    private void ReleaseUnmanagedResources()
    {
        _redis?.Dispose();
    }

    protected virtual void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();
        if (disposing)
        {
            _redis?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~Connection()
    {
        Dispose(false);
    }
}
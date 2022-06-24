using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Mirero.DAQ.Infrastructure.Locking;

public sealed class AdvisoryLockProvider : LockProviderBase
{
    private const int AlreadyHeldReturnCode = 103;

    private readonly DbConnection _dbConnection;
    private readonly TimeSpan _timeout;

    public AdvisoryLockProvider(DbContext dbContext, TimeSpan timeout, ILogger logger) : this(
        () => dbContext.Database.GetDbConnection(), timeout, logger)
    {
    }

    public AdvisoryLockProvider(Func<DbConnection> dbConnectionFactory, TimeSpan timeout, ILogger logger)
    {
        _dbConnection = dbConnectionFactory();
        _timeout = timeout;
        Logger = logger;
    }

    public override ILogger Logger { get; }

    public override async Task<ILock?> TryAcquireAsync(string resource, CancellationToken cancellationToken = default) =>
        await TryAcquireAsync(resource: resource, timeout: TimeSpan.Zero, isShared: false,
            cancellationToken: cancellationToken);

    public override async Task<ILock> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default) =>
        await TryAcquireAsync(resource: resource, timeout ?? _timeout, isShared: false,
            cancellationToken: cancellationToken).ThrowTimeoutIfNull<ILock>(resource);

    public override async Task<ILock>
        AcquireReadLockAsync(string resource, double timeoutSeconds, CancellationToken cancellationToken = default) =>
        await TryAcquireAsync(resource: resource, timeoutSeconds < 0 ? _timeout : TimeSpan.FromSeconds(timeoutSeconds),
            isShared: true, cancellationToken: cancellationToken).ThrowTimeoutIfNull<ILock>(resource);

    public override async Task<ILock>
        AcquireWriteLockAsync(string resource, double timeoutSeconds, CancellationToken cancellationToken = default) =>
        await TryAcquireAsync(resource: resource, timeoutSeconds < 0 ? _timeout : TimeSpan.FromSeconds(timeoutSeconds),
            isShared: false, cancellationToken: cancellationToken).ThrowTimeoutIfNull<ILock>(resource);

    public async Task<ILock?> TryAcquireAsync(string resource, TimeSpan timeout, bool isShared = false,
        CancellationToken cancellationToken = default)
    {
        var key = new AdvisoryLockKey(resource, true);
        Logger.LogInformation("AdvisoryLockKey - Resource : {resource}, Key : {key}.", resource, key.Key);

        //var conn = _dbConnectionFactory();
        //if (conn.State == ConnectionState.Closed)
        //{
        //    await conn.OpenAsync(cancellationToken);
        //}

        if (_dbConnection.State == ConnectionState.Closed)
            await _dbConnection.OpenAsync(cancellationToken);

        //var _dbConnection = _dbConnection.Database.GetDbConnection();

        //await _dbConnection.Database.OpenConnectionAsync(cancellationToken);

        await using var acquireCommand = this.CreateAcquireCommand(_dbConnection, key, timeout, isShared);

        var acquireCommandResult = 0;
        try
        {
            acquireCommandResult = (int)(await acquireCommand.ExecuteScalarAsync(cancellationToken) ?? 0);
            //acquireCommandResult = (int)(acquireCommand.ExecuteScalar() ?? 0);
        }
        catch (Exception ex)
        {
            if (ex is PostgresException postgresException)
            {
                switch (postgresException.SqlState)
                {
                    // lock_timeout error code from https://www.postgresql.org/docs/10/errcodes-appendix.html
                    case "55P03":
                        return null;
                    // deadlock_detected error code from https://www.postgresql.org/docs/10/errcodes-appendix.html
                    case "40P01":
                        throw new InvalidOperationException(
                            $"The request for the distributed lock failed with exit code '{postgresException.SqlState}' (deadlock_detected)",
                            ex);
                }
            }
        }

        ILock? result = null;
        switch (acquireCommandResult)
        {
            case 0: result = null; break;
            case 1: result = new AdvisoryLock(resource, key, this, Logger, isShared); break;

            case AlreadyHeldReturnCode:
                switch (timeout.Milliseconds)
                {
                    case 0:
                        result = null; break;
                    case -1:
                        throw new InvalidOperationException("Attempted to acquire a lock that is already held on the same connection");
                    default:
                        await Task.Delay(timeout, cancellationToken).ConfigureAwait(false);
                        result = null; break;
                }
                break;
            default:
                throw new InvalidOperationException($"Unexpected return code {acquireCommandResult}");
        }

        //if (result == null)
        //{
        //    await _dbConnection.DisposeAsync().ConfigureAwait(false);
        //}

        return result;
    }

    public override async Task ReleaseAsync(ILock @lock, bool isTry, CancellationToken cancellationToken = default)
    {
        var advisoryLock = (AdvisoryLock) @lock;
        await ReleaseAsync(advisoryLock.LockKey, isTry, advisoryLock.IsShared, cancellationToken);
    }

    public async Task ReleaseAsync(ILockKey key, bool isTry, bool isShared = false, CancellationToken cancellationToken = default)
    {
        //var conn = _dbConnectionFactory();
        //if (_dbConnection.State == ConnectionState.Closed)
        //{
        //    await _dbConnection.OpenAsync(cancellationToken);
        //}

        await using var command = _dbConnection.CreateCommand(); 
        command.CommandText =
            $"SELECT pg_catalog.pg_advisory_unlock{(isShared ? "_shared" : string.Empty)}({AddKeyParametersAndGetKeyArguments(command, key)})";

        //bool result;
        //lock (_lock)
        //{
        var result = (bool)(await command.ExecuteScalarAsync(cancellationToken) ?? false);
            //result = (bool)(command.ExecuteScalar() ?? false);

            if (!isTry && !result)
        {
            throw new InvalidOperationException("Attempted to release a lock that was not held");
        }

        //if (_dbConnection.State == ConnectionState.Open)
        //{
        //    await _dbConnection.CloseAsync();
        //}
    }

    protected override async Task DisposeAsyncCore()
    {
        await _dbConnection.DisposeAsync();
    }

    private DbCommand CreateAcquireCommand(DbConnection connection, AdvisoryLockKey key, TimeSpan timeout, bool isShared)
    {
        var cmd = connection.CreateCommand();

        var cmdText = new StringBuilder();

        cmdText.AppendLine("SET LOCAL statement_timeout = 0;");

        cmdText.AppendLine($"SET LOCAL lock_timeout = {timeout.Milliseconds};");

        // 기존 커넥션이 아닌 경우
        //cmdText.Append($@"
        //        SELECT 
        //            CASE WHEN EXISTS(
        //                SELECT * 
        //                FROM pg_catalog.pg_locks l
        //                JOIN pg_catalog.pg_database d
        //                    ON d.oid = l.database
        //                WHERE l.locktype = 'advisory' 
        //                    AND {AddPGLocksFilterParametersAndGetFilterExpression(cmd, _key)} 
        //                    AND l.pid = pg_catalog.pg_backend_pid() 
        //                    AND d.datname = pg_catalog.current_database()
        //            ) 
        //                THEN {AlreadyHeldReturnCode}
        //            ELSE
        //                "
        //);

        //AppendAcquireFunctionCall();

        //cmdText.AppendLine().Append("END");

        cmdText.Append("SELECT ");

        AppendAcquireFunctionCall();

        cmdText.Append(" AS result");

        cmd.CommandText = cmdText.ToString();
        cmd.CommandTimeout = timeout.Milliseconds;

        return cmd;

        void AppendAcquireFunctionCall()
        {
            // creates an expression like
            // pg_try_advisory_lock(@key1)::int
            // OR (SELECT 1 FROM (SELECT pg_advisory_lock(@key)) f)
            var isTry = timeout.Milliseconds == 0;
            if (!isTry) { cmdText.Append("(SELECT 1 FROM (SELECT "); }
            cmdText.Append("pg_catalog.pg");
            if (isTry) { cmdText.Append("_try"); }
            cmdText.Append("_advisory");
            cmdText.Append("_lock");
            if (isShared) { cmdText.Append("_shared"); }
            cmdText.Append('(').Append(AddKeyParametersAndGetKeyArguments(cmd, key)).Append(')');
            cmdText.Append(isTry ? "::int" : ") f)");
        }
    }

    private static string AddKeyParametersAndGetKeyArguments(DbCommand command, ILockKey key)
    {
        AddCommandParameter(command, "key", key.Key, DbType.Int64);
        return "@key";
    }

    private static void AddCommandParameter(DbCommand command, string name, object value, DbType? dbType)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        if (dbType != null) parameter.DbType = dbType.Value;
        command.Parameters.Add(parameter);
    }
}
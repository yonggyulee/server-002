using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Gds.Constants;
using Mirero.DAQ.Infrastructure.Database.Gds;
using Mirero.DAQ.Infrastructure.Grpc.Extensions;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.DownloadGdsStream;

public class DownloadGdsStreamHandler : IRequestHandler<DownloadGdsStreamCommand>
{
    private readonly ILogger _logger;
    private readonly GdsDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly ILockProvider _lockProvider;

    public DownloadGdsStreamHandler(ILogger<DownloadGdsStreamHandler> logger,
        IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory, IFileStorage fileStorage,
        IPostgresLockProviderFactory lockProviderFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _lockProvider = lockProviderFactory.CreateLockProvider(_dbContext) ??
                        throw new ArgumentNullException(nameof(lockProviderFactory));
    }

    public async Task<Unit> Handle(DownloadGdsStreamCommand command, CancellationToken cancellationToken)
    {
        var responseStream = command.ResponseStream;
        var request = command.Request;

        var gds = await _dbContext.Gds.Include(
                    g => g.Volume)
                .SingleOrDefaultAsync(
                    g => g.Id == request.GdsId,
                    cancellationToken)
            ;

        if (gds == null)
        {
            throw new InvalidOperationException($"존재하지 않는 GDS Id 값(={request.GdsId}) 입니다.");
        }

        await using (await _lockProvider.AcquireReadLockAsync(
                         GenerateLockId<Domain.Gds.Protos.V1.Gds>(gds.Id),
                         request.LockTimeoutSec,
                         cancellationToken: cancellationToken))
        {
            if (gds.Status is GdsStatus.Success)
            {
                await using var fs = _fileStorage.OpenReadFileStream(
                        Path.Combine(
                            gds.Volume.Uri,
                            gds.Id.ToString(),
                            gds.Filename)
                    )
                    ;
                await fs.WriteGrpcServerStreamWriterAsync(responseStream, request.ChunkSize, cancellationToken);
            }
            else
            {
                throw new InvalidOperationException($"Gds Status : {gds.Status} 확인 해주세요.");
            }
        }

        return Unit.Value;
    }

    private static string GenerateLockId<TEntity>(object id)
    {
        return typeof(TEntity).Name + id;
    }
}
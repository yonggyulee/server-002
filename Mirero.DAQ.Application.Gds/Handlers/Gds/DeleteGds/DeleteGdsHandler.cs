using Google.Protobuf.WellKnownTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Domain.Gds.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Gds;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using Mirero.DAQ.Application.Gds.UriGenerator;

namespace Mirero.DAQ.Application.Gds.Handlers.Gds.DeleteGds;

public class DeleteGdsHandler : IRequestHandler<DeleteGdsCommand, Empty>
{
    private readonly ILogger _logger;
    private readonly GdsDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly ILockProvider _lockProvider;
    private readonly IUriGenerator _uriGenerator;

    public DeleteGdsHandler(ILogger<DeleteGdsRequest> logger,
        IFileStorage fileStorage,
        IDbContextFactory<GdsDbContextPostgreSQL> dbContextFactory,
        IPostgresLockProviderFactory lockProviderFactory,
        IUriGenerator uriGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _dbContext = dbContextFactory.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _uriGenerator = uriGenerator?? throw new ArgumentNullException(nameof(uriGenerator));
        _lockProvider = lockProviderFactory.CreateLockProvider(_dbContext) ??
                        throw new ArgumentNullException(nameof(lockProviderFactory));
    }

    public async Task<Empty> Handle(DeleteGdsCommand command, CancellationToken cancellationToken)
    {
        // TODO 지우려고자 하는 Gds가 로딩중인 상태인지 확인이 필요한지? FloorPlan 개발 후 수정 필요
        
        var request = command.Request;
        var gds = await _dbContext.Gds.Include(g => g.Volume)
                .SingleOrDefaultAsync(g => g.Id == request.GdsId, cancellationToken);
        
        if (gds == null)
            throw new InvalidOperationException($"존재하지 않는 GDS Id 값(={request.GdsId}) 입니다.");

        var uri = _uriGenerator.GetGdsUri(gds.Volume.Uri, gds.Id, gds.Filename);

        await using (await _lockProvider.AcquireReadLockAsync(
                         GenerateLockId<Domain.Gds.Protos.V1.Gds>(gds.Id),
                         request.LockTimeoutSec,
                         cancellationToken: cancellationToken))
        {
            _dbContext.Gds.Remove(gds);

            if (_fileStorage.FileExists(uri))
                await _fileStorage.DeleteFile(uri, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return new Empty();
    }
    
    private static string GenerateLockId<TEntity>(object id)
    {
        return typeof(TEntity).Name + id;
    }
}
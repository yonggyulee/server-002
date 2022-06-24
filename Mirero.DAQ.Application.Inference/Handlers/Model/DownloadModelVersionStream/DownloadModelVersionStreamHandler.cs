using Google.Protobuf;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Domain.Common.Protos;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ModelDto = Mirero.DAQ.Domain.Inference.Protos.V1.Model;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.DownloadModelVersionStream;

public class DownloadModelVersionStreamHandler : IRequestHandler<DownloadModelVersionStreamCommand>
{
    private readonly ILogger<DownloadModelVersionStreamHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly ILockProvider _lockProvider;
    private readonly IUriGenerator _uriGenerator;
    
    public DownloadModelVersionStreamHandler(ILogger<DownloadModelVersionStreamHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IFileStorage fileStorage,
        IPostgresLockProviderFactory lockProviderFactory, IMapper mapper,
        IUriGenerator uriGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }
        _dbContext = dbContextFactory.CreateDbContext();
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        if (lockProviderFactory == null)
        {
            throw new ArgumentNullException(nameof(lockProviderFactory));
        }
        _lockProvider = lockProviderFactory.CreateLockProvider(_dbContext);
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
    }

    public async Task<Unit> Handle(DownloadModelVersionStreamCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var responseStream = command.ResponseStream;

        var modelVersion =
            await _dbContext.ModelVersions.Include(mv => mv.Model.Volume)
                .SingleOrDefaultAsync(mv => mv.Id == request.ModelVersionId, cancellationToken) ??
            throw new NotImplementedException();

        await using var @lock = await _lockProvider.AcquireReadLockAsync(_lockProvider.GenerateLockId<ModelDto>(modelVersion.ModelId),
            request.LockTimeoutSec,
            cancellationToken: cancellationToken);

        var uri = _uriGenerator.GetModelVersionUri(
            modelVersion.Model.Volume.Uri, 
            modelVersion.Model.ModelName,
            modelVersion.Version, 
            modelVersion.Filename);

        await using var fs = _fileStorage.OpenReadFileStream(uri);
        var response = new DownloadModelVersionResponse
        {
            ModelVersionId = request.ModelVersionId,
            Info = new DataInfo {FileSize = fs.Length, Filename = uri}
        };
        var chunkSize = request.ChunkSize > fs.Length ? fs.Length : request.ChunkSize;
        var offset = 0;
        var buffer = new byte[chunkSize];
        var length = buffer.Length;
        var fileLength = fs.Length;
        var chunkNum = 0;
        while (await fs.ReadAsync(buffer, 0, length, cancellationToken) != 0)
        {
            response.Info.ChunkNum = chunkNum;
            response.Info.ChunkSize = length;
            response.Buffer = ByteString.CopyFrom(buffer);

            await responseStream.WriteAsync(response);
            offset += length;
            length = (int) Math.Min(chunkSize, fileLength - offset);
            chunkNum++;
        }

        return default;
    }
}
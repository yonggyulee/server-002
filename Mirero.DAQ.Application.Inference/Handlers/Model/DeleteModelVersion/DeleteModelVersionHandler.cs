using Google.Protobuf.WellKnownTypes;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ModelDto = Mirero.DAQ.Domain.Inference.Protos.V1.Model;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.DeleteModelVersion;

public class DeleteModelVersionHandler : IRequestHandler<DeleteModelVersionCommand, Empty>
{
    private readonly ILogger<DeleteModelVersionHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly ILockProvider _lockProvider;
    private readonly IUriGenerator _uriGenerator;

    public DeleteModelVersionHandler(ILogger<DeleteModelVersionHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IFileStorage fileStorage,
        IPostgresLockProviderFactory lockProviderFactory,
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

    public async Task<Empty> Handle(DeleteModelVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var selectedModelVersion = await _dbContext.ModelVersions.Include(m => m.Model.Volume)
                                       .SingleOrDefaultAsync(m => m.Id == request.ModelVersionId,
                                           cancellationToken: cancellationToken)
                                   ?? throw new NotImplementedException();

        await using var @lock =
            await _lockProvider.AcquireWriteLockAsync(_lockProvider.GenerateLockId<ModelDto>(selectedModelVersion.ModelId),
                request.LockTimeoutSec,
                cancellationToken: cancellationToken);

        var uri = _uriGenerator.GetModelVersionUri(
            selectedModelVersion.Model.Volume.Uri,
            selectedModelVersion.Model.ModelName,
            selectedModelVersion.Version,
            selectedModelVersion.Filename);

        var deletedFileBuffer = await _fileStorage.DeleteFileAsync(uri, cancellationToken);

        try
        {
            _dbContext.ModelVersions.Remove(selectedModelVersion);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await _fileStorage.SaveFileAsync(uri, deletedFileBuffer, cancellationToken);
            throw;
        }

        //return _mapper.From(selectedModelVersion).AdaptToType<ModelVersion>();
        return new Empty();
    }
}
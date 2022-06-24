using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mirero.DAQ.Application.Inference.UriGenerator;
using Mirero.DAQ.Domain.Inference.Protos.V1;
using Mirero.DAQ.Infrastructure.Database.Inference;
using Mirero.DAQ.Infrastructure.Locking;
using Mirero.DAQ.Infrastructure.Storage;
using ModelDto = Mirero.DAQ.Domain.Inference.Protos.V1.Model;

namespace Mirero.DAQ.Application.Inference.Handlers.Model.UpdateModelVersion;

public class UpdateModelVersionHandler : IRequestHandler<UpdateModelVersionCommand, ModelVersion>
{
    private readonly ILogger<UpdateModelVersionHandler> _logger;
    private readonly InferenceDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;
    private readonly ILockProvider _lockProvider;
    private readonly IUriGenerator _uriGenerator;
    
    public UpdateModelVersionHandler(ILogger<UpdateModelVersionHandler> logger,
        IDbContextFactory<InferenceDbContextPostgreSQL> dbContextFactory, IFileStorage fileStorage,
        IPostgresLockProviderFactory lockProviderFactory, IMapper mapper, IUriGenerator uriGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (dbContextFactory == null)
        {
            throw new ArgumentNullException(nameof(dbContextFactory));
        }

        _dbContext = dbContextFactory.CreateDbContext();
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        if (lockProviderFactory == null)
        {
            throw new ArgumentNullException(nameof(lockProviderFactory));
        }

        _lockProvider = lockProviderFactory.CreateLockProvider(_dbContext);
        _uriGenerator = uriGenerator ?? throw new ArgumentNullException(nameof(uriGenerator));
    }

    public async Task<ModelVersion> Handle(UpdateModelVersionCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        var selectedModelVersion = await _dbContext.ModelVersions.Include(m => m.Model.Volume)
                                       .SingleOrDefaultAsync(
                                           m => m.Id == request.Id,
                                           cancellationToken: cancellationToken)
                                   ?? throw new NotImplementedException();

        var lockIds = new List<string>
        {
            _lockProvider.GenerateLockId<ModelDto>(request.ModelId),
            _lockProvider.GenerateLockId<ModelDto>(selectedModelVersion.ModelId)
        };

        await using var @lock =
            await _lockProvider.AcquireWriteLockAsync(lockIds, request.LockTimeoutSec, cancellationToken: cancellationToken);

        var isChangedModel = selectedModelVersion.ModelId != request.ModelId;

        var isChangedUri = isChangedModel
                           || selectedModelVersion.Version != request.Version
                           || selectedModelVersion.Filename != request.Filename;

        var (originUri, newUri) = ("", "");

        if (isChangedUri)
        {
            newUri = isChangedModel ? (await _dbContext.Models.Include(m => m.Volume)
                .Select(m => new
                {
                    m.Id,
                    Uri = _uriGenerator.GetModelVersionUri(
                                m.Volume.Uri,
                                m.ModelName,
                                request.Version,
                                request.Filename, false)
                })
                .SingleOrDefaultAsync(m => m.Id == request.ModelId, cancellationToken)
                                       ?? throw new NotImplementedException()).Uri
                : _uriGenerator.GetModelVersionUri(
                    selectedModelVersion.Model.Volume.Uri,
                    selectedModelVersion.Model.ModelName,
                    request.Version,
                    request.Filename);

            originUri = _uriGenerator.GetModelVersionUri(
                selectedModelVersion.Model.Volume.Uri,
                selectedModelVersion.Model.ModelName,
                selectedModelVersion.Version,
                selectedModelVersion.Filename);

            _fileStorage.MoveFile(originUri, newUri, true);
        }

        try
        {
            _mapper.From(request).AdaptTo(selectedModelVersion);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            if (isChangedUri)
            {
                _fileStorage.MoveFile(newUri, originUri, true);
            }
            throw;
        }

        return _mapper.From(selectedModelVersion).AdaptToType<ModelVersion>();
    }
}